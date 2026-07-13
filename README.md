# CS710S PC Demo App

![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET Framework 4.8](https://img.shields.io/badge/.NET%20Framework-4.8-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Windows Forms](https://img.shields.io/badge/Windows%20Forms-0078D6?style=for-the-badge&logo=windows&logoColor=white)
![Platform](https://img.shields.io/badge/Platform-Windows%20x86-0078D6?style=for-the-badge&logo=windows&logoColor=white)
![Visual Studio 2022](https://img.shields.io/badge/Visual%20Studio-2022-5C2D91?style=for-the-badge&logo=visual-studio&logoColor=white)
![USB HID](https://img.shields.io/badge/Interface-USB%20HID-6DB33F?style=for-the-badge)

## Introduction

**CS710S PC Demo App** is a Windows desktop utility for the [Convergence Systems Limited (CSL)](https://www.convergence.com.hk/) **CS710S** handheld RFID reader, connected to the PC over **USB HID**.

It is a hands-on demo and maintenance tool that lets you connect to a reader, view its status, exercise its RFID / barcode / Bluetooth features, and upgrade its firmware — all from a single window.

## Requirements

- Windows PC (the app is a 32-bit / x86 build)
- .NET Framework 4.8 runtime
- A CSL **CS710S** RFID reader with a USB cable
- `AtUsbHid.dll` (included; must sit next to the executable)

## Usage

### Connect to a reader

1. Plug the CS710S reader into the PC with a USB cable.
2. Launch **CS710S PC Demo App**.
3. In the **Connection** panel, pick the reader from the device drop-down list and click **Connect**.
4. Once connected, the **Info** panel fills in with the reader's details. Click **Disconnect** to release the reader.

### View reader information

The **Info** panel shows, after connecting:

- **Atmel Firmware Version** (reader microcontroller)
- **Bluetooth Firmware Version**
- **RFID Version**
- **Battery Level** — click **Get Battery Level** to refresh (shown in mV)
- **Trigger State** — click **Get Trigger State** to read the trigger button
- **Device Name** — click **Get Device Name** to read it, or type a new name and click **Set Device Name** (reboot the reader to apply)

### Reader functions

The **Reader** panel opens dedicated windows for the reader's main features:

- **RFID** — run tag inventory and read/write tag memory
- **Barcode** — trigger and read the barcode scanner
- **Bluetooth HID configuration** — configure the reader's Bluetooth keyboard-emulation (HID) behavior

### Upgrade firmware

Three upgrade sections are available, each with an **Upgrade Image** button:

| Section | Firmware file pattern |
|---------|-----------------------|
| **Atmel Firmware Upgrade** | `CS710ATMEL_*.bin` |
| **Bluetooth Firmware Upgrade** | `CS710_CC2652xx_APP_*.bin` (matched to the reader model) |
| **RFID Firmware Upgrade** | `ex10_app*.bin` |

To upgrade:

1. Connect to the reader.
2. In the relevant section, click **Upgrade Image**.
3. Choose the firmware `.bin` file in the file dialog.
4. A progress window tracks the upload and reports **success** or **failure** when finished.
5. Power-cycle / reboot the reader when prompted to apply the new firmware.

> **Note:** Make sure the firmware file matches your reader and model. The file dialog filters to the expected file-name pattern for each firmware type.

## Building from source

1. Open `CS710 PC Demo App/CS710 PC Demo App.sln` in **Visual Studio 2022**.
2. To build the installer project (`CS710 PC Demo App Installer`), install the free **Microsoft Visual Studio Installer Projects 2022** extension (Extensions → Manage Extensions).
3. Select the **`Debug|x86`** or **`Release|x86`** configuration and build.
4. Ensure `AtUsbHid.dll` is present next to the built executable before running.

## License

Copyright © Convergence Systems Limited. See [`Copyright.txt`](CS710%20PC%20Demo%20App/Copyright.txt) for details.
