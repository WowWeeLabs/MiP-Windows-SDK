using BluetoothRobotControlLib.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Bluetooth.GenericAttributeProfile;
using Windows.Devices.Enumeration;
using Windows.Foundation;
using BluetoothRobotControlLib.Common.Sevices;
using System.Diagnostics;

namespace MipWindowLib.MipRobot
{
    public class MipRobot : BluetoothRobot
    {
        public string DeviceName { get; set; }
        public int ProductId { get; set; }

        public int BatteryLevel { get; private set; }
        public event EventHandler<int> MipBatteryLevelHandler;

        public MipRobotConstants.POSITION_VALUE Position { get; private set; }
        public event EventHandler<MipRobotConstants.POSITION_VALUE> MipPositionHandler;

        public BluetoothRobotConstants.ACTIVIATION_STATUS ToyActivationStatus { get; private set; }
        public event EventHandler<BluetoothRobotConstants.ACTIVIATION_STATUS> MipToyActivationStatusHandler;

        public int VoiceFirmwareVersion { get; private set; }
        public event EventHandler<int> MipVoiceFirmwareVersionHandler;

        public int HardwareVersion { get; private set; }
        public event EventHandler<int> MipHardwareVersionHandler;

        public int FirmwareVersionId { get; private set; }
        public event EventHandler<int> MipFirmwareVersionIdHandler;

        public DateTime FirmwareVersionDate { get; private set; }
        public event EventHandler<DateTime> MipFirmwareVersionDateHandler;

        public int VolumeLevel { get; private set; }
        public event EventHandler<int> MipVolumeLevelHandler;

        public MipRobotConstants.PING_RESPONSE BootMode { get; private set; }
        public event EventHandler<MipRobotConstants.PING_RESPONSE> MipBootModeHandler;

        public byte GameMode { get; private set; }
        public EventHandler<byte> MipGameModeHandler;

        public byte WeightLevel { get; private set; }
        public EventHandler<byte> MipWeightLevelHandler;

        public bool LeaningForward { get; private set; }
        public EventHandler<bool> MipLeaningForwardHandler;

        public MipRobot() : base()
        {
        }

        public MipRobot(DeviceInformation deviceInfo) : base(deviceInfo)
        {
            //TODO: update DeviceName

            //TODO: update ProductId
        }

        public override async Task<bool> Connect()
        {
            bool result = await base.Connect();

            DeviceName = SendDataServiceInfo.Name;

            return result;
        }

        /*
        Read Info
        */

        public IAsyncAction ReadMipStatus()
        {
            return SendMipCommand(MipRobotConstants.COMMAND_CODE.GET_STATUS).AsAsyncAction();
        }

        public IAsyncAction ReadMipHardwareVersion()
        {
            return SendMipCommand(MipRobotConstants.COMMAND_CODE.GET_HARDWARE_VERSION).AsAsyncAction();
        }

        public IAsyncAction ReadMipFirmwareVersion()
        {
            return SendMipCommand(MipRobotConstants.COMMAND_CODE.GET_SOFTWARE_VERSION).AsAsyncAction();
        }

        public IAsyncAction ReadMipVolumeLevel()
        {
            return SendMipCommand(MipRobotConstants.COMMAND_CODE.GET_VOLUME_LEVEL).AsAsyncAction();
        }

        public IAsyncAction ReadMipActivationStatus()
        {
            return SendMipCommand(MipRobotConstants.COMMAND_CODE.GET_TOY_ACTIVATED_STATUS).AsAsyncAction();
        }

        public IAsyncAction ReadMipSensorWeightLevel()
        {
            return SendMipCommand(MipRobotConstants.COMMAND_CODE.GET_WEIGHT_LEVEL).AsAsyncAction();
        }

        /*
        Write Info
        */
        public IAsyncAction ResetMipProductActivated()
        {
            return SetMipProductionActivation((byte)BluetoothRobotConstants.ACTIVIATION_STATUS.FACTORY_DEFAULT);
        }

        public IAsyncAction ActivateMipProduct()
        {
            return SetMipProductionActivation(ToyActivationStatus | BluetoothRobotConstants.ACTIVIATION_STATUS.ACTIVATE);
        }

        public IAsyncAction ActivateMipProductAndUpload()
        {
            return SetMipProductionActivation(ToyActivationStatus | BluetoothRobotConstants.ACTIVIATION_STATUS.ACTIVATE_SENT_TO_FLURRY);
        }

        public IAsyncAction ActivateMipHackerAndUpload()
        {
            return SetMipProductionActivation(ToyActivationStatus | BluetoothRobotConstants.ACTIVIATION_STATUS.HACKER_UART_USED_SENT_TO_FLURRY);
        }

        public IAsyncAction SetMipVolumeLevel(byte volume)
        {
            return SendMipCommand(MipRobotConstants.COMMAND_CODE.SET_VOLUME_LEVEL, volume).AsAsyncAction();
        }

        /*
        Device command
        */
        public IAsyncAction RebootMipAndWriteFlash(bool writeFlash, bool writeIO)
        {
            if (writeIO)
            {
                //TODO missing on android version
                throw new Exception("writeIO can't be true");
            }
            else
            {
                if (writeFlash)
                {
                    return SendMipCommand(MipRobotConstants.COMMAND_CODE.REBOOT, (byte)MipRobotConstants.RESET_MCU.RESET_AND_FORCE_BOOT_LOADER).AsAsyncAction();
                }
                else
                {
                    return SendMipCommand(MipRobotConstants.COMMAND_CODE.REBOOT, (byte)MipRobotConstants.RESET_MCU.NORMAL_RESET).AsAsyncAction();
                }
            }
        }

        /*
        Driving command
        */
        public IAsyncAction MipDrive(Vector2 vector)
        {
            byte driveValue = (byte)Math.Round(Math.Min(1, Math.Abs(vector.Y)) * 32);
            byte turnValue = (byte)Math.Round(Math.Min(1, Math.Abs(vector.X)) * 32);

            driveValue += (vector.Y > 0) ? (byte)MipRobotConstants.DRIVE_CONTINOUS_VALUE.FW_SPEED1 : (byte)MipRobotConstants.DRIVE_CONTINOUS_VALUE.BW_SPEED1;
            turnValue += (vector.X > 0) ? (byte)MipRobotConstants.DRIVE_CONTINOUS_VALUE.RIGHT_SPEED1 : (byte)MipRobotConstants.DRIVE_CONTINOUS_VALUE.LEFT_SPEED1;

            return SendMipCommand(MipRobotConstants.COMMAND_CODE.DRIVE_CONTINOUS, driveValue, turnValue).AsAsyncAction();
        }

        public IAsyncAction MipDriveToward(int time, int speed)
        {
            return MipDriveWithRateAndTime(true, speed, time);
        }

        public IAsyncAction MipDriveBackward(int time, int speed)
        {
            return MipDriveWithRateAndTime(false, speed, time);
        }

        public IAsyncAction MipPunchLeftWithSpeed(int speed)
        {
            return MipTurnWithRate(-90, speed);
        }

        public IAsyncAction MipPunchRightWithSpeed(int speed)
        {
            return MipTurnWithRate(90, speed);
        }

        public IAsyncAction MipTurnLeftByDegrees(int degrees, int speed)
        {
            return MipTurnWithRate(-degrees, speed);
        }

        public IAsyncAction MipTurnRightByDegress(int degrees, int speed)
        {
            return MipTurnWithRate(degrees, speed);
        }
        public IAsyncAction MipDriveDistanceByCm(int distanceInCm, int degrees=0)
        {
            return SendMipCommand(
                    MipRobotConstants.COMMAND_CODE.DRIVE_FIXED_DISTANCE,
                    //direction
                    (distanceInCm > 0) ? MipRobotConstants.DRIVE_DIRECTION_FORWARD : MipRobotConstants.DRIVE_DIRECTION_BACKWARD,
                    //distance
                    (byte)Math.Abs(distanceInCm),
                    //turn direction
                    (degrees > 0) ? MipRobotConstants.DRIVE_TURN_DIRECTION_CLOCKWISE : MipRobotConstants.DRIVE_TURN_DIRECTION_ANTI_CLOCKWISE,
                    //angle
                    (byte)(Math.Abs(degrees) << 8),
                    (byte)(Math.Abs(degrees) & 0x00ff)
                ).AsAsyncAction();
        }

        public IAsyncAction MipFalloverWithSytle(MipRobotConstants.POSITION_VALUE style)
        {
            return SendMipCommand(MipRobotConstants.COMMAND_CODE.SHOULD_FALLOVER).AsAsyncAction();
        }

        public IAsyncAction MipPing()
        {
            return SendMipCommand(MipRobotConstants.COMMAND_CODE.CHECK_BOOT_MODE).AsAsyncAction();
        }

        /*
        LEDs
        */
        public IAsyncAction SetMipHeadLeds(MipRobotConstants.HEAD_LED led1, MipRobotConstants.HEAD_LED led2, MipRobotConstants.HEAD_LED led3, MipRobotConstants.HEAD_LED led4)
        {
            return SendMipCommand(MipRobotConstants.COMMAND_CODE.SET_HEAD_LED, (byte)led1, (byte)led2, (byte)led3, (byte)led4).AsAsyncAction();
        }

        public IAsyncAction SetMipChestLedFlashingWithColor(byte red, byte green, byte blue, byte timeoff)
        {
            return SendMipCommand(MipRobotConstants.COMMAND_CODE.FLASH_CHEST_RGB_LED, red, green, blue, timeoff).AsAsyncAction();
        }

        public IAsyncAction SetMipChestLedWithColor(byte red, byte green, byte blue, byte fadeIn)
        {
            return SendMipCommand(MipRobotConstants.COMMAND_CODE.SET_CHEST_RGB_LED, red, green, blue, fadeIn).AsAsyncAction();
        }

        /*
        IR
        */
        public IAsyncAction MipTransmitIRGameDataWithGameType(byte gameType, byte mipId, short gameData, byte powerOfDistance)
        {
            return MipTransmitIRCommand(powerOfDistance, gameType, mipId, (byte)(gameData >> 8), (byte)(gameData & 0xff));
        }

        public IAsyncAction MipTransmitIRCommand(byte distanceInCm, params byte[] bytes)
        {
            if (distanceInCm <= 0 || distanceInCm > 120)
            {
                throw new Exception("distance should be 1-120");
            }

            if (bytes.Length <= 0 || bytes.Length > 4)
            {
                throw new Exception("bytes size should be 1-32");
            }

            return SendMipCommand(
                    MipRobotConstants.COMMAND_CODE.TRANSMIT_IR_COMMAND,
                    (byte)((bytes.Length >= 4)? bytes[3] : 0x00),
                    (byte)((bytes.Length >= 3)? bytes[2] : 0x00),
                    (byte)((bytes.Length >= 2)? bytes[1] : 0x00),
                    (byte)((bytes.Length >= 1)? bytes[0] : 0x00),
                    (byte)(bytes.Length * 8),
                    distanceInCm
                ).AsAsyncAction();
        }

        /*
        Sound
        */
        public IAsyncAction PlayMipSound(MipRobotSound sound)
        {
            if (sound.Volume == -1)
            {
                return SendMipCommand(MipRobotConstants.COMMAND_CODE.PLAY_SOUND, (byte)sound.File, sound.Delay).AsAsyncAction();
            }
            else
            {
                return SendMipCommand(MipRobotConstants.COMMAND_CODE.PLAY_SOUND, (byte)sound.File, sound.Delay, (byte)sound.Volume, 0x00).AsAsyncAction();
            }
        }

        //private functions

        private IAsyncAction SetMipProductionActivation(BluetoothRobotConstants.ACTIVIATION_STATUS status)
        {
            ToyActivationStatus = status;
            return SendMipCommand(MipRobotConstants.COMMAND_CODE.SET_TOY_ACTIVATED_STATUS, (byte)status).AsAsyncAction();
        }

        private IAsyncAction MipDriveWithRateAndTime(bool toward, int rate, int time)
        {
            MipRobotConstants.COMMAND_CODE type = toward ? MipRobotConstants.COMMAND_CODE.DRIVE_FORWARD_WITH_TIME : MipRobotConstants.COMMAND_CODE.DRIVE_BACKWARD_WITH_TIME;
            byte r = (byte)Math.Min(rate, 30);
            byte t = (byte)Math.Min(time / 7, 255);

            return SendMipCommand(type, r, t).AsAsyncAction();
        }

        private IAsyncAction MipTurnWithRate(int degree, int rate)
        {
            MipRobotConstants.COMMAND_CODE type = degree < 0 ? MipRobotConstants.COMMAND_CODE.TURN_LEFT_BY_ANGLE : MipRobotConstants.COMMAND_CODE.TURN_RIGHT_BY_ANGLE;
            byte d = (byte)(Math.Min(Math.Abs(degree), 360) / 4);
            byte r = (byte)Math.Min(rate, 24);

            return SendMipCommand(type, d, r).AsAsyncAction();
        }

        private async Task<bool> SendMipCommand(MipRobotConstants.COMMAND_CODE type, params byte[] data)
        {
            return await base.SendCommand((byte)type, data);
        }

        //Implement Override Functions

        protected override void DidNotifyByReceiveDataCharacteristic(byte[] data)
        {
            switch (data[0])
            {
                case (byte)MipRobotConstants.COMMAND_CODE.GET_GAME_MDOE:
                    if (data.Length == 2)
                    {
                        GameMode = data[1];

                        MipGameModeHandler?.Invoke(this, GameMode);
                    }
                    break;
                case (byte)MipRobotConstants.COMMAND_CODE.GET_STATUS:
                    if (data.Length == 3)
                    {
                        BatteryLevel = data[1];
                        Position = BaseService.ConvertEnumFromInt<MipRobotConstants.POSITION_VALUE>(data[2]);

                        MipBatteryLevelHandler?.Invoke(this, BatteryLevel);
                        MipPositionHandler?.Invoke(this, Position);
                    }
                    break;
                case (byte)MipRobotConstants.COMMAND_CODE.GET_HARDWARE_VERSION:
                    if (data.Length == 3)
                    {
                        for (byte i = 0; i < MipRobotConstants.VOICE_FIRWARE_MAPPING.Length; i++)
                        {
                            if (data[1] > MipRobotConstants.VOICE_FIRWARE_MAPPING[i])
                            {
                                VoiceFirmwareVersion = i;
                            }
                        }
                        HardwareVersion = data[2];

                        MipVoiceFirmwareVersionHandler?.Invoke(this, VoiceFirmwareVersion);
                        MipHardwareVersionHandler?.Invoke(this, HardwareVersion);
                    }
                    break;
                case (byte)MipRobotConstants.COMMAND_CODE.GET_SOFTWARE_VERSION:
                    if (data.Length == 5)
                    {
                        FirmwareVersionDate = new DateTime(2000 + data[1], data[2], data[3]);
                        FirmwareVersionId = data[4];

                        MipFirmwareVersionDateHandler?.Invoke(this, FirmwareVersionDate);
                        MipFirmwareVersionIdHandler?.Invoke(this, FirmwareVersionId);
                    }
                    break;
                case (byte)MipRobotConstants.COMMAND_CODE.GET_VOLUME_LEVEL:
                    if (data.Length == 2)
                    {
                        VolumeLevel = data[1];

                        MipVolumeLevelHandler?.Invoke(this, VolumeLevel);
                    }
                    break;
                case (byte)MipRobotConstants.COMMAND_CODE.GET_TOY_ACTIVATED_STATUS:
                    if (data.Length == 2)
                    {
                        ToyActivationStatus = BaseService.ConvertEnumFromInt<BluetoothRobotConstants.ACTIVIATION_STATUS>(data[1]);

                        if (ToyActivationStatus.HasFlag(BluetoothRobotConstants.ACTIVIATION_STATUS.ACTIVATE))
                        {
                            Task.Run(async () =>
                            {
                                await ActivateMipProduct();
                            });
                        }

                        MipToyActivationStatusHandler?.Invoke(this, ToyActivationStatus);
                    }
                    break;
                case (byte)MipRobotConstants.COMMAND_CODE.RECEIVE_IR_COMMAND:
                    //TODO callback
                    break;
                case (byte)MipRobotConstants.COMMAND_CODE.GET_WEIGHT_LEVEL:
                    if (data.Length == 2)
                    {
                        WeightLevel = data[1];
                        LeaningForward = WeightLevel < 0xd3;

                        MipWeightLevelHandler?.Invoke(this, WeightLevel);
                        MipLeaningForwardHandler?.Invoke(this, LeaningForward);
                    }
                    break;
                case (byte)MipRobotConstants.COMMAND_CODE.CHECK_BOOT_MODE:
                    if (data.Length == 2)
                    {
                        BootMode = BaseService.ConvertEnumFromInt<MipRobotConstants.PING_RESPONSE>(data[1]);

                        MipBootModeHandler?.Invoke(this, BootMode);
                    }
                    else if (data.Length == 1)
                    {
                        BootMode = MipRobotConstants.PING_RESPONSE.NORMAL_ROM_NO_BOOT_LOADER;

                        MipBootModeHandler?.Invoke(this, BootMode);
                    }
                    break;
            }
        }
    }
}
