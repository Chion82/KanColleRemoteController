#Environment Requirement
* Operating System: Windows Server 2012, Windows 8.1 (tested) (Windows 7/2008: Compatibility unknown)
* To enable remote control over Internet, your server must acquire a public network IP address which can be globally accessed.

> Note: If your PC is connected to Internet via LAN, try enabling DMZ or mapping over port 3000 on the LAN router.

#Dependencies
* Node.js
* Internet Expolrer 11 (or higher version) [Already installed on Win8/2012 by default]
* .NET Framework 4.5 [Already installed on Win8/2012 by default]

#Installation

##Server Setup
* Initialize game account info 
	- Launch ```KanColleViewer/KanColleViewer.exe``` and log in the game with your Google account. 
	- Now the logging info is cached in IE Cookies so that the controller can automatically log in the game with your account. (Currently only logging with Google accounts is supported)

>  The auto game-logging-in function depends on the recognition of the "Log in with Google account" button. However due to the difference of resolution, OS or IE version, the actual appearance of the button rendered on your environment may differ from mine. Thus you may need to capture your own sample image of the "Log in with Google account" button to enhance recognition accuracy. 

* Capture the sample image of "Log in with Google account" button
	- Launch ```大漠综合工具.exe```
	- Click ```抓图(CTRL+1)``` to capture the button as ```.bmp``` image
	- Replace ```web_login_google.bmp``` with the image file you just captured.

> The image you captured may look like this:
> ![](https://bytebucket.org/Chion82/kancolleremotecontroller/raw/master/web_login_google.bmp)

* Change the dashboard admin password
	- Open ```WebServer/auth/auth.js``` with Nodepad (Sublime Text is better if possible)
	- Edit the first line of ```auth.js```. Modify the value of the ```PASS``` variable to your desired password.
	- Sample:
	```var PASS = 'admin';	 //The admin password is "admin"```

* Run server
	- Launch a Command Prompt with Administrator privilege
	- Locale into the ```WebServer``` directory
	- Run ```node ./bin/www```
 
##Client Usage
* On your laptop/smart phone, browse http://server_ip:3000 and login to dashboard.
* Enjoy!

> Note: 
>* You should run the game by clicking "Restart Game" in the dashboard at least 1 time or you get "Loading" in the Game Status panel all the time.
>* After clicking "Restart Game" button in the dashboard, your server need 3~5 minutes to start the game and the auto game script (KanCollePlayerSimulator). Meanwhile do not perform any mouse/keyboard actions on the server.

#Reference Projects
* [KanColleViewer](https://github.com/Grabacr07/KanColleViewer)
* 奏的舰队脚本(新版)V3.3.3.3