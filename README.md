# DXToolsTeamControl
MS Dynamics CRM plugin that catching the publish requests and alerts all user running the windows tray app.

Reason for creation:
- MS Dynamics Online has the ability to lock when muliple developers are on the same enviroment doing publishing and importing. This tool helps everyone work in a more aware state. The need to constantly shout out to the rest of the team "I'M PUBLISHING!!" is removed.

Requirements:
- Azure mobile service instance to act as the secured signalr hub
- Azure based ms dynamics crm instance with the provided solution imported and running connecting to the hub 
- The windows tray app connecting to the hub

Todo:
- Alerts to include dynamics enviroment url/domain
- Alerts to include the enitity that is being published
- Add "publish complete" check to crm plugin
- Include crm "Import" messages/events plugin
- Client publishing direct to crm, input and store crm login, drag and drop zip/select solution to import
- Queuing publish (if possible and not slowing publish down too much)
- List all users online
- Improve error logging
- Create mobile client apps, just for the fun of it - android/ios/windows10
