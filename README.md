# Magic Mirror

### Code Structure

  The program's code structure is similar to a tree, with the files App.xaml and App.xaml.cs acting as our root. From there, all other files are working independently as leaf nodes, and one can navigate in between pages and classes by using the global object this.Frame. The project's folders are background resources for all other classes to use to make their development more convenient. This is also where the APIs we chose to use are located.

### Setup

  To start, you need to download Microsoft Visual Studios. Here is the latest [version](https://visualstudio.microsoft.com/downloads/); the community download version would be fine. Make sure to also install the Universal Windows App Development Tools option, as shown [here](https://docs.microsoft.com/en-us/windows/uwp/get-started/get-set-up).

### Build
  
  Once all installations are done, clone, or download, the git repository [here](https://github.com/tltl4866/MM). Unzip the file, and open Microsoft Visual Studios. Navigate to the File tab option on the upper left corner; click open, and choose "Project/Solution...". Navigate to where you saved the unzipped folder. Open the folder and click on "Senior_Project_V1.sln". Once opened, navigate to the "Tools" tab option on the top bar; hover over "NuGet Package Manager" and click "Manage NuGet Packages for Solutions...". Within this repository, refer to the image file titled, "NuGet Packages". Search and download these files, if not already present, inside the NuGet Package Manager view in MS Visual Studios. Then, on the menu bar, click "Project" and click "Senior_Project_V1 Properties...". Making sure that the "Application" is selected on the left panel, change the target version to Windows 10 Fall Creators Update. Then click "Build" on the left panel, and make sure that the "Platform Target" is set to x64. Then on the "Solutions Explorer" pane on the right hand side, click on "Package.appmanifest". Once open, click on "Capabilities" and choose:
  - Background Media Playback
  - Internet (Client)
  - Location
  - Microphone
  - Pictures Library
  - Removable Storage
  - Webcame
  
  Once all these are done, simply click on the green play button on the top menu, and choose local machine. This will then start open the program.
