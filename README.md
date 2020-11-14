
# JsonToCsharp
A xrmtoolbox plugin for Dyanmic36 webapi to convert the json to CSharp models. You can also use this tool to convert any json to c# models.
This tool requires only **OAuth or Certificate** types connection to get crm entities json.

## Connection Types
Since this plugin connects to CE webapi so by default it requires **OAuth or Certifcate** type connections in xrmtoolbox.
<br/>For example regarding available OAuth connections in xrmtools are mentioned below:

![xrmtoolbox connections](https://github.com/yesadahmed/xrmtoolboxAddins/blob/main/JsonToCSharp/images/Conn1.png)

Some examples are as follows.

![xrmtoolbox connections](https://github.com/yesadahmed/xrmtoolboxAddins/blob/main/JsonToCSharp/images/sdkcontrol.png)

![xrmtoolbox connections](https://github.com/yesadahmed/xrmtoolboxAddins/blob/main/JsonToCSharp/images/conneciont.PNG)
 AuthType=OAuth;Username=jsmith@contoso.onmicrosoft.com; Password=passcode;
Url=https://contoso:8080/Test;AppId=<GUID>;RedirectUri=app://<GUID>; LoginPrompt=Never

<br/><br/>
For more help about CE connection strings. [here](https://docs.microsoft.com/en-us/previous-versions/dynamicscrm-2016/developers-guide/mt608573(v=crm.8)?redirectedfrom=MSDN) <br/>
See more info about CE WebAPI authentication methods. [here](https://docs.microsoft.com/en-us/dynamics365/customerengagement/on-premises/developer/webapi/authenticate-web-api). <br/>
For more help you can raise an issuse [here](https://github.com/yesadahmed/xrmtoolboxAddins/issues)

Adnan Samuel
