using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CS710_PC_Client
{
    static class Constants
    {
        public const int TIMER_READ = 1; //ms
        
        public const int ATMEL_IMAGE_SIZE = 16384;
        public const int ATMEL_IMAGE_TOTAL_SUBPART = 144; //size/114+1
        public const int ATMEL_BOOTLOADER_SIZE = 9216;
        public const int ATMEL_BOOTLOADER_TOTAL_SUBPART = 81; //size/114+1

        public const int BT_IMAGE_SIZE = 124928;
        public const int BT_IMAGE_TOTAL_SUBPART = 1952; //size/64

        public const int RFID_IMAGE_SIZE = 203776;
        public const int RFID_IMAGE_TOTAL_SUBPART = 6368; //size/32

        public const byte PREFIX = 0xA7;
        public const byte CONNECTION_BT = 0xB3;
        public const byte CONNECTION_USB = 0xE6;
        public const byte TYPE_RFID = 0xC2;
        public const byte TYPE_BARCODE = 0x6A;
        public const byte TYPE_NOTIFY = 0xD9;
        public const byte TYPE_ATMEL = 0xE8;
        public const byte TYPE_BT = 0x5F;
        public const byte LINK_DOWN = 0x37;
        public const byte LINK_UP = 0x9E;
        public const byte RESERVE = 0x82;

        public const UInt16 COMMAND_PREFIX = 0x80B3;
        public const UInt16 COMMAND_REPLY_PREFIX = 0x51E2;
        public const UInt16 UPLINK_PREFIX = 0x49DC;

        // RFID commands
        public const UInt16 RFID_ReadReg = 0x1471;
        public const UInt16 RFID_WriteReg = 0x9A06;
        public const UInt16 SCSLRFIDStartSimpleInventory = 0x10A1;
        public const UInt16 SCSLRFIDStartCompactInventory = 0x10A2;
        public const UInt16 SCSLRFIDStartSelectInventory = 0x10A3;
        public const UInt16 SCSLRFIDStartMBInventory = 0x10A4;
        public const UInt16 SCSLRFIDStartSelectMBInventory = 0x10A5;
        public const UInt16 SCSLRFIDStartSelectCompactInventory = 0x10A6;
        public const UInt16 SCSLRFIDStartAccessInventory = 0x10A7;
        public const UInt16 SCSLRFIDStartSelectAccessInventory = 0x10A8;
        public const UInt16 SCSLRFIDStopOperation = 0x10AE;
        public const UInt16 SCSLRFIDReadMB = 0x10B1;
        public const UInt16 SCSLRFIDWriteMB = 0x10B2;
        public const UInt16 SCSLRFIDWriteMBAny = 0x10B3;
        public const UInt16 SCSLRFIDBlockWriteMB = 0x10B5;
        public const UInt16 SCSLRFIDBlockWriteMBAny = 0x10B6;
        public const UInt16 SCSLRFIDLock = 0x10B7;
        public const UInt16 SCSLRFIDKill = 0x10B8;
        public const UInt16 SCSLRFIDAuthenticate = 0x10B9;
        public const UInt16 SCSLEnableDuplicateElimination = 0x10D1;
        public const UInt16 SCSLDisableDuplicateElimination = 0x10D2;
        public const UInt16 SCSLClearTagCacheTable = 0x10D3;
        public const UInt16 SCSLUploadTagCacheTableToHost = 0x10D4;
        public const UInt16 SCSLRFIDRegisterReset = 0x10D5;
        public const UInt16 SCSLEx10Reset = 0x10D6;
        public const UInt16 SCSLRFIDCircuitReset = 0x10D7;
        public const UInt16 SCSLRFIDGetNoiseFloor = 0x10D8;

        public const UInt16 CSLPreloadEx10Image = 0x8831;
        public const UInt16 CSLImageStartBurnEx10Image = 0x8832;
        public const UInt16 CSLValidateImage = 0x8833;
        public const UInt16 CSLWriteInfoPage = 0x8835;
        public const UInt16 CSLReadInfoPage = 0x8836;
        public const UInt16 CSLSyncTime = 0x8840;

        // Uplink packet
        public const UInt16 csl_tag_read_epc_only_new = 0x3001;
        public const UInt16 csl_tag_read_epc_only_recurrent = 0x3002;
        public const UInt16 csl_tag_read_multibank_new = 0x3003;
        public const UInt16 csl_tag_read_multibank_recurrent_index_only = 0x3004;
        public const UInt16 csl_tag_read_compact = 0x3006;
        public const UInt16 csl_miscellaneous_event = 0x3007;
        public const UInt16 csl_operation_complete = 0x3008;
        public const UInt16 csl_access_complete = 0x3009;

        // CSL registers
        public const UInt16 REG_LoopStyle = 0x3000;
        public const UInt16 REG_HopStyle = 0x3008;
        public const UInt16 REG_RegulatoryNoEmissionTime = 0x3010;
        public const UInt16 REG_CountryEnum = 0x3014;
        public const UInt16 REG_FrequencyChannelIndex = 0x3018;
        public const UInt16 REG_AntennaPortConfig = 0x3030;
        public const UInt16 REG_SelectConfiguration = 0x3140;
        public const UInt16 REG_MultibankReadConfig = 0x3270;
        public const UInt16 REG_MultibankWriteConfig = 0x3290;
        public const UInt16 REG_AccessPassword = 0x38A6;
        public const UInt16 REG_KillPassword = 0x38AA;
        public const UInt16 REG_LockMask = 0x38AE;
        public const UInt16 REG_LockAction = 0x38B0;
        public const UInt16 REG_DuplicateEliminationRollingWindow = 0x3900;
        public const UInt16 REG_TagCacheTableCurrentSize = 0x3902;
        public const UInt16 REG_TagCacheStatus = 0x3904;
        public const UInt16 REG_EventPacketUplinkEnable = 0x3906;
        public const UInt16 REG_IntraPacketDelay = 0x3908;
        public const UInt16 REG_RssiFilteringConfig = 0x390A;
        public const UInt16 REG_RssiThreshold = 0x390C;
        public const UInt16 REG_AuthenticateConfig = 0x390E;
        public const UInt16 REG_AuthenticateMessage = 0x3912;
        public const UInt16 REG_AuthenticateRepLen = 0x3944;
        public const UInt16 REG_CurrentMode = 0x3952;
        public const UInt16 REG_NoiseFloorFrequency = 0x3956;
        public const UInt16 REG_NoiseFloorValue = 0x395A;

        // CSL OEM registers
        public const UInt16 OEM_ConfigEnable = 0x5040;
        public const UInt16 OEM_CountryEnum = 0x5042;
        public const UInt16 OEM_FrequencyChannelIndex = 0x5044;
        public const UInt16 OEM_AntennaPortConfig = 0x5046;
        public const UInt16 OEM_DuplicateEliminationRollingWindow = 0x5148;
        public const UInt16 OEM_HidFormat = 0x5150;
        public const UInt16 OEM_HidPrefix1 = 0x5152;
        public const UInt16 OEM_HidPrefix2 = 0x5154;
        public const UInt16 OEM_HidSuffix1 = 0x5156;
        public const UInt16 OEM_HidSuffix2 = 0x5158;
        public const UInt16 OEM_HidIntervalDelay = 0x51F2;
        public const UInt16 OEM_HidReportPerInterval = 0x51F4;
        public const UInt16 OEM_HidBarcodeSupport = 0x51F6;
    }
}
