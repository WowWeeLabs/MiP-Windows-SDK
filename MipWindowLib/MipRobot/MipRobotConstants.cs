using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MipWindowLib.MipRobot
{
    public class MipRobotConstants
    {
        public const int PRODUCT_ID = 5;

        public enum COMMAND_CODE : byte
        {
            SET_IR_REMOTE_ONOFF = 0x10,
            GET_IR_REMOTE_ONOFF = 0x11,

            SET_RADAR_MODE = 0x0C,
            GET_RADAR_MODE = 0x0D,

            SET_DETECTION_MODE = 0x0E,
            GET_DETECTION_MODE = 0x0F,

            SET_GESTURE_MODE = 0x0A,
            GET_GESTURE_MODE = 0x0B,

            CHECK_BOOT_MODE = 0xFF,

            SHOULD_POWER_OFF = 0xFB,
            REBOOT = 0xFB,
            SHOULD_SLEEP = 0xFA,
            SHOULD_DISCONNECT_APP_MODE = 0xFE,
            SHOULD_FORCE_BLE_DISCONNECT = 0xFC,

            SET_USER_DATA = 0x12,
            GET_USER_DATA = 0x13,

            SET_GAME_MODE = 0x76,
            GET_GAME_MDOE = 0x78,

            SET_VOLUME_LEVEL = 0x15,
            GET_VOLUME_LEVEL = 0x16,

            GET_SOFTWARE_VERSION = 0x14,
            GET_HARDWARE_VERSION = 0x19,

            SHOULD_FALLOVER = 0x08,

            PLAY_SOUND = 0x06,

            STOP = 0x77,

            SHAKE_DETECTED = 0x1A,

            GET_ODEMETER = 0x85,
            RESET_ODEMETER = 0x86,

            GET_STATUS = 0x79,

            GET_WEIGHT_LEVEL = 0x81,
            GET_CHEST_RGB_LED = 0x83,
            SET_CHEST_RGB_LED = 0x84,
            FLASH_CHEST_RGB_LED = 0x89,
            GET_HEAD_LED = 0x8A,
            SET_HEAD_LED = 0x8B,

            TRANSMIT_IR_COMMAND = 0x8C,
            RECEIVE_IR_COMMAND = 0x03,

            DRIVE_CONTINOUS = 0x78,
            DRIVE_FIXED_DISTANCE = 0x70,
            DRIVE_FORWARD_WITH_TIME = 0x71,
            DRIVE_BACKWARD_WITH_TIME = 0x72,

            TURN_LEFT_BY_ANGLE = 0x73,
            TURN_RIGHT_BY_ANGLE = 0x74,

            SET_HACKER_UART_MODE = 0x01,
            GET_HACKER_UART_MODE = 0x02,

            SEND_WOWWEE_IR_DONGLE_CODE = 0x8C,

            CLAPS_DETECTED = 0x1D,
            GET_CLAPS_DETECTION_STATUS = 0x1E,
            CLAPS_DETECTION_STATUS = 0x1F,
            SET_CLAPS_DETECTION_TIMING = 0x20,

            HACKER_UART_CONNECTED_STATUS_UPDATED = 0x1C,
            SET_MIP_DETECTION_MODE = 0x0E,
            GET_MIP_DETECTION_MODE = 0x0F,
            OTHER_MIP_DETECTED = 0x04,

            GET_UP_FROM_POSITION = 0x23,

            GET_TOY_ACTIVATED_STATUS = 0x21,
            SET_TOY_ACTIVATED_STATUS = 0x22
        }

        /*
		Drive continuous value
		*/
        public enum DRIVE_CONTINOUS_VALUE : byte
        {
            FW_SPEED1 = 0x00,
            BW_SPEED1 = 0x20,
            LEFT_SPEED1 = 0x60,
            RIGHT_SPEED1 = 040
        }

        public enum SOUND_FILE : byte
        {
            ONEKHZ_500MS_8K16BIT = 0x01,
            ACTION_BURPING,
            ACTION_DRINKING,
            ACTION_EATING,
            ACTION_FARTING_SHORT,
            ACTION_OUT_OF_BREATH,
            BOXING_PUNCHCONNECT_1,
            BOXING_PUNCHCONNECT_2,
            BOXING_PUNCHCONNECT_3,
            FREESTYLE_TRACKING_1,
            MIP_1,
            MIP_2,
            MIP_3,
            MIP_APP,
            MIP_AWWW,
            MIP_BIG_SHOT,
            MIP_BLEH,
            MIP_BOOM,
            MIP_BYE,
            MIP_CONVERSE_1,
            MIP_CONVERSE_2,
            MIP_DROP,
            MIP_DUNNO,
            MIP_FALL_OVER_1,
            MIP_FALL_OVER_2,
            MIP_FIGHT,
            MIP_GAME,
            MIP_GLOAT,
            MIP_GO,
            MIP_GOGOGO,
            MIP_GRUNT_1,
            MIP_GRUNT_2,
            MIP_GRUNT_3,
            MIP_HAHA_GOT_IT,
            MIP_HI_CONFIDENT,
            MIP_HI_NOT_SURE,
            MIP_HI_SCARED,
            MIP_HUH,
            MIP_HUMMING_1,
            MIP_HUMMING_2,
            MIP_HURT,
            MIP_HUUURGH,
            MIP_IN_LOVE,
            MIP_IT,
            MIP_JOKE,
            MIP_K,
            MIP_LOOP_1,
            MIP_LOOP_2,
            MIP_LOW_BATTERY,
            MIP_MIPPEE,
            MIP_MORE,
            MIP_MUAH_HA,
            MIP_MUSIC,
            MIP_OBSTACLE,
            MIP_OHOH,
            MIP_OH_YEAH,
            MIP_OOPSIE,
            MIP_OUCH_1,
            MIP_OUCH_2,
            MIP_PLAY,
            MIP_PUSH,
            MIP_RUN,
            MIP_SHAKE,
            MIP_SIGH,
            MIP_SINGING,
            MIP_SNEEZE,
            MIP_SNORE,
            MIP_STACK,
            MIP_SWIPE_1,
            MIP_SWIPE_2,
            MIP_TRICKS,
            MIP_TRIIICK,
            MIP_TRUMPET,
            MIP_WAAAAA,
            MIP_WAKEY,
            MIP_WHEEE,
            MIP_WHISTLING,
            MIP_WHOAH,
            MIP_WOO,
            MIP_YEAH,
            MIP_YEEESSS,
            MIP_YO,
            MIP_YUMMY,
            MOOD_ACTIVATED,
            MOOD_ANGRY,
            MOOD_ANXIOUS,
            MOOD_BORING,
            MOOD_CRANKY,
            MOOD_ENERGETIC,
            MOOD_EXCITED,
            MOOD_GIDDY,
            MOOD_GRUMPY,
            MOOD_HAPPY,
            MOOD_IDEA,
            MOOD_IMPATIENT,
            MOOD_NICE,
            MOOD_SAD,
            MOOD_SHORT,
            MOOD_SLEEPY,
            MOOD_TIRED,
            SOUND_BOOST,
            SOUND_CAGE,
            SOUND_GUNS,
            SOUND_ZINGS,
            SHORT_MUTE_FOR_STOP,
            FREESTYLE_TRACKING_2
        };

        /*
		Drive Direction values
		*/
        public const byte DRIVE_TURN_DIRECTION_CLOCKWISE = 0x00;
        public const byte DRIVE_TURN_DIRECTION_ANTI_CLOCKWISE = 0x01;
        public const byte DRIVE_DIRECTION_FORWARD = 0x00;
        public const byte DRIVE_DIRECTION_BACKWARD = 0x01;

        /*
		Position Value
		*/
        public enum POSITION_VALUE : byte
        {
            ON_BACK = 0x00,
            FACEDOWN,
            UP_RIGHT,
            PICKED_UP,
            HANDSTAND,
            FACEDOWN_TRAY,
            BACK_WITH_KICKSTAND
        }

        public const byte BOARDCAST_DATA_AVATAR_ICON = 0x00;

        /*
		Head LED value
		*/
        public enum HEAD_LED : byte
        {
            OFF = 0x00,
            ON,
            BLINK_SLOW,
            BLINK_FAST,
            FADE_IN
        }

        /*
		Ping Response
		*/
        public enum PING_RESPONSE : byte
        {
            NORMAL_ROM_NO_BOOT_LOADER = 0x00,
            NORMAL_ROM_HAS_BOOT_LOADER,
            BOOT_LOADER
        }

        /*
		Reset Mcu
		*/
        public enum RESET_MCU : byte
        {
            NORMAL_RESET = 0x01,
            RESET_AND_FORCE_BOOT_LOADER
        }

        /*
        voice firmware version
        */
        public static readonly byte[] VOICE_FIRWARE_MAPPING = new byte[] { 0, 6, 14, 22, 30, 38, 46, 54, 62, 70, 78, 86, 94 };
    }
}
