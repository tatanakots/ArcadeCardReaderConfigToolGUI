//���ߣ�Qinh
//��AIд��
#include <stdio.h>
#include <stdlib.h>
#include <stdint.h>
#include <string.h>
#include <windows.h>
#include <conio.h>
#include <stdbool.h>
#include <ctype.h>

#define HIGH_BAUDRATE 115200 // �߲�����
#define LOW_BAUDRATE 38400 // �Ͳ�����
#define DATA_LENGTH 12 // �ظ����ݵĳ���
#define CHECKSUM_OFFSET 0xE0 // У��λ��ƫ����
#define LED_MIN 2 // LED���ȵ���Сֵ
#define LED_MAX 255 // LED���ȵ����ֵ

HANDLE hPort; // ���ھ��
DCB dcb; // ���ڲ����ṹ��
COMMTIMEOUTS timeouts; // ���ڳ�ʱ�ṹ��
char comPort[13];
uint8_t recv_buffer[DATA_LENGTH]; // �������ݻ�����
uint8_t system_setting_buffer[2] = {0}; // ϵͳ���û�����
BOOL high_baudrate_mode = FALSE; // �߲�����ģʽ��־
uint8_t change_highbaudrate_mode = 0;
BOOL led_enabled = FALSE; // LED���ñ�־
uint8_t led_brightness = 0; // LED����
uint8_t firmware_version = 0; // �̼��汾��
uint8_t hardware_version = 0; // Ӳ���汾��

enum {
    CMD_READ_EEPROM = 0xf6,
    CMD_WRITE_EEPROM = 0xf7,
    CMD_SW_MODE = 0xf8,
    CMD_READ_MODE = 0xf9,
};

enum {
    SEGA_MODE = 0,
    SPICE_MODE = 1,
    NAMCO_MODE = 2,
    TEST_MODE = 3,
    RAW_MODE = 4,
};

enum {
    STATUS_OK = 0x00,
    STATUS_CARD_ERROR = 0x01,
    STATUS_NOT_ACCEPT = 0x02,
    STATUS_INVALID_COMMAND = 0x03,
    STATUS_INVALID_DATA = 0x04,
    STATUS_SUM_ERROR = 0x05,
    STATUS_INTERNAL_ERROR = 0x06,
    STATUS_INVALID_FIRM_DATA = 0x07,
    STATUS_FIRM_UPDATE_SUCCESS = 0x08,
    STATUS_COMP_DUMMY_2ND = 0x10,
    STATUS_COMP_DUMMY_3RD = 0x20,
};

typedef union {
    uint8_t bytes[128];
    struct {
        uint8_t frame_len;
        uint8_t addr;
        uint8_t seq_no;
        uint8_t cmd;
        uint8_t payload_len;
        union {
            uint8_t mode;
            struct {
                uint8_t eeprom_data[2];     //ϵͳ�ڲ�����
                uint8_t mapped_IDm[8];
                uint8_t target_accesscode[10];
            };
        };
    };
} packet_request_t;

typedef union {
    uint8_t bytes[128];
    struct {
        uint8_t frame_len;
        uint8_t addr;
        uint8_t seq_no;
        uint8_t cmd;
        uint8_t status;
        uint8_t payload_len;
        union {
            uint8_t mode;
            uint8_t version[1];  // CMD_GET_FW_VERSION,CMD_GET_HW_VERSION,CMD_EXT_BOARD_INFO
            uint8_t block[16];   // CMD_MIFARE_READ
            uint8_t eeprom_data[4];
        };
    };
} packet_response_t;

packet_request_t req;
packet_response_t res;

// �򿪴��ڵĺ���������ֵΪBOOL����ʾ�Ƿ�ɹ�
BOOL open_port()
{
    // �򿪴���
    hPort = CreateFile(comPort, GENERIC_READ | GENERIC_WRITE, 0, NULL, OPEN_EXISTING, 0, NULL);
    if (hPort == INVALID_HANDLE_VALUE)
    {
        printf("�޷��򿪴���%s��\n", comPort);
        return FALSE;
    }

    // ��ȡ���ڲ���
    if (!GetCommState(hPort, &dcb))
    {
        printf("�޷���ȡ����%s�Ĳ�����\n", comPort);
        CloseHandle(hPort);
        return FALSE;
    }

    // ���ô��ڲ���
    dcb.BaudRate = HIGH_BAUDRATE; // ���ò�����Ϊ�߲�����
    dcb.ByteSize = 8; // ��������λΪ8
    dcb.Parity = NOPARITY; // ��������żУ��
    dcb.StopBits = ONESTOPBIT; // ����ֹͣλΪ1
    if (!SetCommState(hPort, &dcb))
    {
        printf("�޷����ô���%s�Ĳ�����\n", comPort);
        CloseHandle(hPort);
        return FALSE;
    }

    // ��ȡ���ڳ�ʱ
    if (!GetCommTimeouts(hPort, &timeouts))
    {
        printf("�޷���ȡ����%s�ĳ�ʱ��\n", comPort);
        CloseHandle(hPort);
        return FALSE;
    }

    // ���ô��ڳ�ʱ
    timeouts.ReadIntervalTimeout = 50; // ���ö�ȡ�����ʱΪ50����
    timeouts.ReadTotalTimeoutConstant = 1000; // ���ö�ȡ�ܳ�ʱ����Ϊ1000����
    timeouts.ReadTotalTimeoutMultiplier = 10; // ���ö�ȡ�ܳ�ʱ����Ϊ10����
    timeouts.WriteTotalTimeoutConstant = 100; // ����д���ܳ�ʱ����Ϊ100����
    timeouts.WriteTotalTimeoutMultiplier = 10; // ����д���ܳ�ʱ����Ϊ10����
    if (!SetCommTimeouts(hPort, &timeouts))
    {
        printf("�޷����ô���%s�ĳ�ʱ��\n", comPort);
        CloseHandle(hPort);
        return FALSE;
    }

    // ���سɹ�
    return TRUE;
}

void close_port()
{
    CloseHandle(hPort);
}

void add_data(uint8_t* buffer,uint8_t send_byte,uint8_t* len)
{
    buffer[*len] = send_byte;
    memset(len,*len+1,1);
}

BOOL recv_data(uint8_t recv_buffer)
{
    DWORD bytes_read; 
    return ReadFile(hPort, &recv_buffer, 1, &bytes_read, NULL);
}
uint8_t len, r, checksum;
bool escape = false;

uint8_t packet_read() {
    uint8_t recv_buffer[128];
    long unsigned int recv_len;
    ReadFile(hPort, recv_buffer, 128, &recv_len, NULL);
    res.frame_len = 0;
    for (uint8_t i = 0;i<recv_len;i++) {
        r = recv_buffer[i];
        printf("%x ",r);
        if (r == 0xE0) {
            res.frame_len = 0xFF;
            continue;
        }
        if(res.frame_len == 0){
            continue;
        }
        if (res.frame_len == 0xFF) {
            res.frame_len = r;
            len = 0;
            checksum = r;
            continue;
        }
        if (r == 0xD0) {
            escape = true;
            continue;
        }
        if (escape) {
            r++;
            escape = false;
        }
        res.bytes[++len] = r;
        if (len == res.frame_len) {
            return checksum == r ? res.cmd : STATUS_SUM_ERROR;
        }
        checksum += r;
    }
    return 0;
}

void packet_write() {
    uint8_t checksum = 0, len = 0;
    uint8_t send_buffer[128];
    uint8_t send_len;
    if (req.cmd == 0) {
        return;
    }
    add_data(send_buffer,0xE0,&send_len);
    while (len <= req.frame_len) {
        uint8_t w;
        if (len == req.frame_len) {
            w = checksum;
        } else {
            w = req.bytes[len];
            checksum += w;
        }
        if (w == 0xE0 || w == 0xD0) {
            add_data(send_buffer,0xD0,&send_len);
            add_data(send_buffer,--w,&send_len);
        } else {
            add_data(send_buffer,w,&send_len);
        }
        len++;
    }
    // if (!GetCommState(hPort, &dcb))
    // {
    //     printf("A");
    // }
    WriteFile(hPort,send_buffer,send_len, NULL, NULL);
    // if (!GetCommState(hPort, &dcb))
    // {
    //     printf("B");
    // }
    req.cmd = 0;
}

void req_read_eeprom() {
  req.frame_len = 6 + 0;
  req.addr = 0;
  req.seq_no = 0;
  req.cmd = CMD_READ_EEPROM;
  req.payload_len = 0;
  packet_write();
}

void req_write_eeprom(uint8_t* eeprom_data,uint8_t* mapped_IDm,uint8_t* target_accesscode) {
  req.frame_len = 6 + 20;
  req.addr = 0;
  req.seq_no = 0;
  req.cmd = CMD_READ_EEPROM;
  req.payload_len = 20;
  memcpy(req.eeprom_data,eeprom_data,2);
  memcpy(req.mapped_IDm,mapped_IDm,8);
  memcpy(req.target_accesscode,target_accesscode,10);
  packet_write();
}          

void req_change_mode(uint8_t mode) {
  req.frame_len = 6 + 1;
  req.addr = 0;
  req.seq_no = 0;
  req.cmd = CMD_SW_MODE;
  req.payload_len = 1;
  req.mode = mode;
  packet_write();
}

// �޸Ĳ����ʵĺ���������Ϊ�����ʣ�����ֵΪBOOL����ʾ�Ƿ�ɹ�
BOOL change_baudrate(int baudrate)
{
    //DWORD errors;  
    //COMSTAT comStat;  
    //ClearCommError(hPort, &errors, &comStat);
    if (!GetCommState(hPort, &dcb))
    {
        printf("�޷���ȡ����%s�Ĳ�����ERROR:%lu\n", comPort,GetLastError());
        return FALSE;
    }
    // ���ô��ڲ���
    dcb.BaudRate = baudrate; // ���ò�����
    if (!SetCommState(hPort, &dcb))
    {
        printf("�޷����ô���%s�Ĳ�����\n", comPort);
        return FALSE;
    }
    // ���سɹ�
    return TRUE;
}

// ��ȡ�û�����ĺ���������Ϊ��ʾ��Ϣ������ֵΪchar����ʾ�û�������ַ�
char get_user_input(char *prompt)
{
    char input; // �û�������ַ�
    // ��ӡ��ʾ��Ϣ
    printf("%s", prompt);
    // ��ȡ�û�����
    input = getch();
    // �����û�����
    return input;
}

// ��ȡ�û���������ֵĺ���������Ϊ��ʾ��Ϣ������ֵΪint����ʾ�û����������
int get_user_input_number(char *prompt)
{
    char input[256]; // �û�������ַ���
    int number; // �û����������
    // ��ӡ��ʾ��Ϣ
    printf("%s", prompt);
    // ��ȡ�û�����
    fgets(input, 256, stdin);
    // ���Խ�����ת��Ϊ����
    if (sscanf(input, "%d", &number) != 1)
    {
        // ת��ʧ�ܣ�����-1
        return -1;
    }
    // ת���ɹ�����������
    return number;
}
bool convert_string_to_hex(const char* str, uint8_t* output) {

    // ����ַ��������Ƿ�ȫ��Ϊ0~9��A~F
    for (size_t i = 0; i<16; i++) {
        if (!isxdigit(str[i])) {
            return false;
        }
    }

    // ���ַ���ת��Ϊ16���Ʋ��洢��������
    for (size_t i = 0; i < 8; i++) {
        sscanf(str + 2 * i, "%2hhx", &output[i]);
    }

    return true;
}
bool convert_string_to_decimal(const char* str, uint8_t* output) {

    // ����ַ��������Ƿ�ȫ��Ϊ0~9
    for (size_t i = 0; i < 20; i++) {
        if (!isdigit(str[i])) {
            return false;
        }
    }

    // ���ַ���ת��Ϊ10���Ʋ��洢��������
    for (size_t i = 0; i < 20; i++) {
        output[i] = str[i] - '0';
    }

    return true;
}
bool process_array(uint8_t* input, uint8_t* output) {
    for (int i = 0; i < 20; i++) {
        if (input[i] < 0 || input[i] > 9) {
            return false;
        }
    }

    for (int i = 0; i < 10; i++) {
        output[i] = (input[2*i] << 4) | input[2*i + 1];
    }

    return true;
}
// ������
int main(){
    printf("BaudRateTool V10 By Qinh\n");
    printf("�����������޸�Aime�������ڲ����ã�Դ�����https://github.com/QHPaeek/Arduino-Aime-Reader\n");
    printf("������EEPROM�������ޣ��벻ҪƵ���޸ģ�\n");
    int ports;
    while(1)
    {
        ports = get_user_input_number("������������Ķ˿ںţ�����com4����������4�����»س�������");
        if (ports < 0){
            printf("��������Ч�����֣�");
            continue;
        }
        if(ports > 9 ){
        snprintf(comPort, sizeof(comPort), "\\\\.\\COM%d", ports);
        break;
        }
        snprintf(comPort, sizeof(comPort), "COM%d", ports);
        break;
    }
    // �򿪴���
    if (!open_port()){
        printf("�򿪴���ʧ�ܣ�");
        // ��ʧ�ܣ��ȴ��û�����������˳�����
        getch();
        return -1;
    }
    uint8_t mode_rst_cmd[30]={0xaf,0xaf,0xaf,0xaf,0xaf,0xaf,0xaf,0xaf,0xaf,0xaf,0xaf,0xaf,0xaf,0xaf,0xaf,0xaf,0xaf,0xaf,0xaf,0xaf,0xaf,0xaf,0xaf,0xaf,0xaf,0xaf,0xaf,0xaf,0xaf,0xaf};
    change_baudrate(LOW_BAUDRATE);
    WriteFile(hPort, mode_rst_cmd, 30, NULL, NULL);
    Sleep(1);
    change_baudrate(HIGH_BAUDRATE);
    WriteFile(hPort, mode_rst_cmd, 30, NULL, NULL);
    Sleep(1000);
    //��������ͬ�����ʷ�30��0xaf����������ģʽ��ʼ�����ȷ����������ʱ�л���segaģʽ
    // ��COM4�˿���115200���������16��������E0 06 00 00 F6 00 00 FC�����Ҽ����ظ���
    uint8_t send_buffer1[8] = {0xE0,0x06,00,00,0xF6,00,00,0xFC};
    WriteFile(hPort, send_buffer1, 8, NULL, NULL);
	//req_read_eeprom();
    // ��������
    if (packet_read() != CMD_READ_EEPROM)
    {
        // ����ʧ�ܣ�������38400��������������������ݣ����Ҽ����ظ���
        if (!change_baudrate(LOW_BAUDRATE))
        {
            // �޸Ĳ�����ʧ�ܣ��رմ��ڣ��ȴ��û�����������˳�����
            close_port();
            getch();
            return -1;
        }
	    WriteFile(hPort, send_buffer1, 8, NULL, NULL);
        if (packet_read() != CMD_READ_EEPROM)
        {
            // ����ʧ�ܣ��رմ��ڣ��ȴ��û�����������˳�����
            printf("���������Ӵ����������ӣ�\n");
            close_port();
            getch();
            return -1;
        }
    }
    // ��������
    high_baudrate_mode = res.eeprom_data[0] & 0x02; // ��8���ֽڵĵ�2λ��ʾ�߲�����ģʽ
    led_enabled = res.eeprom_data[0] & 0x04; // ��8���ֽڵĵ�3λ��ʾLED����
    led_brightness = res.eeprom_data[1]; // ��9���ֽڱ�ʾLED����
    firmware_version = res.eeprom_data[2]; // ��10���ֽڱ�ʾ�̼��汾��
    hardware_version = res.eeprom_data[3]; // ��11���ֽڱ�ʾӲ���汾��
    // ��ӡ����
    printf("������״̬��Ϣ���£�\n");
    printf("�߲�����ģʽ��%s\n", high_baudrate_mode ? "��" : "��");
    printf("LED���ã�%s\n", led_enabled ? "��" : "��");
    printf("LED���ȣ�%d\n", led_brightness);
    printf("�̼��汾��v%d\n", firmware_version);
	switch(hardware_version)
	{
		case 1:
			printf("Ӳ���汾��ATmega32U4\n");
			break;
		case 2:
			printf("Ӳ���汾��SAMD21\n");
			break;
		case 3:
			printf("Ӳ���汾��ESP8266\n");
			break;
		case 4:
			printf("Ӳ���汾��ESP32\n");
			break;
		case 5:
			printf("Ӳ���汾��AIR001/PY32F002\n");
			break;
		case 6:
			printf("Ӳ���汾��STM32F1\n");
			break;
		case 7:
			printf("Ӳ���汾��STM32F0\n");
			break;
		case 8:
			printf("Ӳ���汾��RP2040\n");
			break;
		case 9:
			printf("Ӳ���汾��ATmega328P\n");
			break;
        case 10:
			printf("Ӳ���汾��ESP32C3\n");
			break;
		default:
			printf("Ӳ���汾��δ֪\n");
			break;
	}
    char choice; 
    uint8_t mode_sw = 0;
    while (1)
    {
	    printf("��ʾ��V4�汾���²�֧�ֶ�������ģʽ��V7�汾���²�֧�ֿ���ӳ�书��\n");
	    printf("��ʾ�������������ģʽ��ֻ��ͨ�������˳���������˳������������޷�������ģʽ����\n");
	    printf("��ʾ�����Ӵ˹��ߺ�������ѱ�����ΪĬ��segaģʽ\n");
        choice = get_user_input("����1�޸�segaģʽ�¶��������ã�����2�����������ģʽ������3����Namcoģʽ������4����Spiceģʽ������5����PN532ֱͨģʽ������n�˳�\n");
        // �ж��û�����
        if (choice == '1' ){
	        mode_sw = 1;
            break;
        }else if (choice == '2' ){
	        mode_sw = 2;
            break;
        }else if (choice == '3' ){
		    req_change_mode(NAMCO_MODE);
		    printf("ģʽ�޸���ɣ���������˳�\n");
        	getch();
        	return -1;
        }else if (choice == '4' ){
		    req_change_mode(SPICE_MODE);
		    printf("ģʽ�޸���ɣ���������˳�\n");
        	getch();
        	return -1;
        }else if (choice == '5' ){
		    req_change_mode(RAW_MODE);
		    printf("ģʽ�޸���ɣ���������˳�\n");
        	getch();
        	return -1;
        }else if (choice == 'n' || choice == 'N'){
            // ����n����N���رմ��ڣ��˳�����
            close_port();
            return 0;
        }else{
            // �����������ݣ���ʾ�û������������룡��
            printf("���������룡\n");
        }
    }
    if (mode_sw == 1){
        if (high_baudrate_mode)
        {
            // �߲�����ģʽ�����ò�����Ϊ115200
            if (!change_baudrate(HIGH_BAUDRATE))
            {
                // �޸Ĳ�����ʧ�ܣ��رմ��ڣ��ȴ��û�����������˳�����
                close_port();
                getch();
                return -1;
            }
        }else{
        // �Ͳ�����ģʽ�����ò�����Ϊ38400
            if (!change_baudrate(LOW_BAUDRATE))
            {
                // �޸Ĳ�����ʧ�ܣ��رմ��ڣ��ȴ��û�����������˳�����
                close_port();
                getch();
                return -1;
            }
        }
        while (1){
            // ��ȡ�û�����
            choice = get_user_input("�Ƿ����ø߲�����ģʽ������y���ã�����n������\n");
            // �ж��û�����
            if (choice == 'y' || choice == 'Y')
            {
                // ����y����Y������system_setting_buffer[0] �ĵڶ�λ��1������ѭ��
                system_setting_buffer[0] |= 0x02;
            change_highbaudrate_mode = 1;
                break;
            }
            else if (choice == 'n' || choice == 'N')
            {
                // ����n����N������ѭ��
            change_highbaudrate_mode = 0;
                break;
            }
            else
            {
                // �����������ݣ���ʾ�û������������룡��
                printf("���������룡\n");
            }
        }
        while (1){
            // ��ȡ�û�����
            choice = get_user_input("�Ƿ�����LED������y���ã�����n������\n");
            // �ж��û�����
            if (choice == 'y' || choice == 'Y')
            {
                // ����y����Y������system_setting_buffer[0] �ĵ���λ��1������ѭ��
                system_setting_buffer[0] |= 0x04;
                break;
            }
            else if (choice == 'n' || choice == 'N')
            {
                // ����n����N������ѭ��
                break;
            }
            else
            {
                // �����������ݣ���ʾ�û������������룡��
                printf("���������룡\n");
            }
        }
        int brightness; // LED����
        while (1)
        {
            // ��ȡ�û����������
            brightness = get_user_input_number("������LED���ȷ�Χ����ΧΪ0~255�����س�����\n");
            // �ж��û�����������Ƿ�Ϸ�
            if (brightness >= 0 && brightness <= 255)
            {
                // �Ϸ������������ֵ����system_setting_buffer[1]������ѭ��
                system_setting_buffer[1] = brightness;
                break;
            }
            else
            {
                // ���Ϸ�����ʾ�û�������������!��
                printf("����������!\n");
            }
        }
        while (1){
            // ��ȡ�û�����
            choice = get_user_input("�Ƿ�������չ����������y���ã�����n������\n");
            // �ж��û�����
            if (choice == 'y' || choice == 'Y')
            {
                // ����y����Y������system_setting_buffer[0] �ĵ�4λ��1������ѭ��
                system_setting_buffer[0] |= 0b10000;
                break;
            }
            else if (choice == 'n' || choice == 'N')
            {
                // ����n����N������ѭ��
                break;
            }
            else
            {
                // �����������ݣ���ʾ�û������������룡��
                printf("���������룡\n");
            }
        }
        while (1){
            // ��ȡ�û�����
            choice = get_user_input("�Ƿ���SPICEģʽ�½���2Pˢ��ģʽ��IIDX�ã�������y���ã�����n������\n");
            // �ж��û�����
            if (choice == 'y' || choice == 'Y')
            {
                // ����y����Y������system_setting_buffer[0] �ĵ�4λ��1������ѭ��
                system_setting_buffer[0] |= 0b1000000;
                break;
            }
            else if (choice == 'n' || choice == 'N')
            {
                // ����n����N������ѭ��
                break;
            }
            else
            {
                // �����������ݣ���ʾ�û������������룡��
                printf("���������룡\n");
            }
        }
        uint8_t card_reflect = 0;
        while (1){
            // ��ȡ�û�����
            choice = get_user_input("�Ƿ����ÿ���ӳ�书�ܣ�����y���ã�����n������\n");
            // �ж��û�����
            if (choice == 'y' || choice == 'Y')
            {
                // ����y����Y������system_setting_buffer[0] �ĵ�4λ��1������ѭ��
                system_setting_buffer[0] |= 0b1000;
            card_reflect = 1;
                break;
            }
            else if (choice == 'n' || choice == 'N')
            {
                // ����n����N������ѭ��
                break;
            }
            else
            {
                // �����������ݣ���ʾ�û������������룡��
                printf("���������룡\n");
            }
        }
        char card_IDm[8];
        char card_accesscode[10] = {0};
        if(card_reflect){	
            printf("�����뱻ת������IDm(16���Ƹ�ʽ��`6λ����ǰ׺���м䲻��Ҫ�ո�,���س�����\n");
            printf("IDm����ͨ�������ߵĶ�������ģʽ��ȡ\n");
            bool transform_result_IDm = false;
            while(!transform_result_IDm){
                char char_buffer[256];
                fgets(char_buffer, 256, stdin);
                if(convert_string_to_hex(char_buffer,card_IDm)){
                    transform_result_IDm = true;
                }
                else{
                    printf("�����ʽ��������������\n");
                }
            }
            printf("������Ŀ��ת�����Ŀ���(10���Ƹ�ʽ��20λ����ǰ׺���м䲻��Ҫ�ո�,���س�����\n");
            bool transform_result_accode = false;
            while(!transform_result_accode){
                char char_buffer[256];
                uint8_t accode_buffer[20];
                fgets(char_buffer, 256, stdin);
                if(convert_string_to_decimal(char_buffer,accode_buffer)) {
                    transform_result_accode = true;
                    process_array(accode_buffer, card_accesscode);
                }
                else{
                    printf("�����ʽ��������������\n");
                }
            }
        }
        if(high_baudrate_mode){
            change_baudrate(115200);
        }else{
            change_baudrate(38400);
        }
        uint8_t uart_send_buffer[28] = {0xE0,0x1A,0x00,0x00,0xF7,0x14,0x0E,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00};
        uart_send_buffer[6] = system_setting_buffer[0]; 
        uart_send_buffer[7] = system_setting_buffer[1]; 
        for(uint8_t i =0;i<8;i++){
            uart_send_buffer[i+8] = card_IDm[i];
        }
        for(uint8_t i =0;i<10;i++){
            uart_send_buffer[i+16] =card_accesscode[i];
        }
        uint16_t checksum_cmd;
        for(uint8_t i =0;i<26;i++){
            uart_send_buffer[27] += uart_send_buffer[i+1];
        }
        DWORD bytes_written;
        //OVERLAPPED overlapped = {0};
        //overlapped.hEvent = CreateEvent(NULL, TRUE, FALSE, NULL);
        //if (!WriteFile(hPort, uart_send_buffer, 28, &bytes_written, &overlapped))
        //{
        //        GetOverlappedResult(hPort, &overlapped, &bytes_written, TRUE);
        //}
        //CloseHandle(overlapped.hEvent);
        WriteFile(hPort, uart_send_buffer,28, &bytes_written, NULL);
        Sleep(3);
        // �ȴ��������ݷ������
        // �ı䴮�ڲ�����
        if (change_highbaudrate_mode){
            // �û�ѡ���˸߲�����ģʽ�����ò�����Ϊ115200
            change_baudrate(HIGH_BAUDRATE);
        }else{
            // �û�û��ѡ��߲�����ģʽ�����ò�����Ϊ38400
            change_baudrate(LOW_BAUDRATE);
        }
        printf("�޸���ɣ���������˳�\n");
        close_port();
        getch();
        return -1;
    }else{
        if (high_baudrate_mode){
            if (!change_baudrate(HIGH_BAUDRATE)){  
                close_port();
                getch();
                return -1;
            }
        }else{
            if (!change_baudrate(LOW_BAUDRATE)){
                close_port();
                getch();
                return -1;
            }
        }
        DWORD bytes_written;
        char buffer[1024];
        DWORD bytesRead;
        uint8_t send_buffer_readtest_cmd[8] = {0xE0 ,0x06 ,00,00,0xF8 ,0x01,0x03,0x02};
        while((WriteFile(hPort, send_buffer_readtest_cmd,8, &bytes_written, NULL) == FALSE));
        while (1) {
            while(ReadFile(hPort, buffer, sizeof(buffer), &bytesRead, NULL) == FALSE);
            system("cls");
            for (DWORD i = 0; i < bytesRead; i++) {
                printf("%c", buffer[i]);
            }
        }
    }
}
