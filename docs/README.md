# SharpLocker

SharpLocker helps get current user credentials by popping a fake Windows lock screen. It is written in C# and uses WPF. Fork from [ReLock](https://github.com/cftad/ReLock)

## Works
* Single/Multiple Monitors, different resolutions
* Windows 10
* Prevents screensaver
* Disables shortcuts
* Checks password validity

![Working SharpLocker](https://github.com/msmidik/SharpLocker/blob/main/docs/screen.png?raw=true)

## How to
* Logic mainly in Views/MainWindow.xaml.cs - uncomment to send creadentials as http request of save to file
* Compile SharpLocker from source via VisualStudio
