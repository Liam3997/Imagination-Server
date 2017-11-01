<img src="https://i.imgur.com/pL7Xgq6.png" width="200" height="200"></img>
# Imagination-Server
A C# server for LEGO Universe, that will (hopefully not?) die.  

*The LEGO Group has not endorsed or authorized the operation of this game and is not liable for any safety issues in relation to the operation of this game*

## Project Info
Imagination Server is an Lego Universe server emulator started by MicleBrick and currently being built-upon and maintained by Liam3997. 

Current goals for the project are:
* Build a fully functioning Lego Universe server emulator
* Build an emulator that can be used by anybody to host their own personal LU servers
* Build a GUI and guide for the emulator that will enable users with little technical know-how to run their own personal LU servers
* Remain completely, 100% open-source, for anybody to contribute to
* Introduce some new faces to the LU development community and the LU community in general!

## More Info
Unlike (most) other LEGO Universe servers, this uses C#, a managed, Object-Oriented programming language. It is run as a single application, and uses SQLite with Fluent NHibernate for storing data.

## Modules
### Base
ImaginationServer.Core - C++ code that provides a wrapper around RakNet  
ImaginationServer.Common - Base C# server code shared between servers
### Servers
ImaginationServer.Auth - Authentication server, and has commands  
ImaginationServer.Auth.Packets - Login Packets, now in C#!  
ImaginationServer.World - World server  
ImaginationServer.World.Packets - Same as .Auth.Packets, except for world.
### Utilities
sd0 utils: (Broken) sd0 compressor/decompressor (Possibly will be removed soon...)
### Other
ImaginationServer.SingleServer - Application that runs the Auth and World servers.  
ImaginationServer.GameLauncher - A launcher/patcher for LU, allows switching boot.cfg with a few clicks (not implemented)  
ImaginationServer.ControlPanel - Same as single server, but with a GUI (not implemented)

## Permissions
Permission has been granted by MicleBrick, both through Discord and through his licensing choices, to use his original code to build this server.  
This project also operates under the provisions [made by Lego to the original LUNI project](http://timtechsoftware.com/wp-content/uploads/2014/09/LU-official.png), that such server emulation is allowed provided that no Lego copyright is infringed and a disclaimer (seen at the top of this README) is provided when referring to the project

## License
This project is licensed under the MIT License, which basically means you can do whatever you want with this code, just as long as you credit the people who have worked so hard on it this far!

## Credits
* MicleBrick for starting this awesome project and providing a solid stepping stone for us to get started on! Also thanks to MicleBrick for the awesome, original logo. 100% credit to him for that.
* LCDR, Humanoid, and everybody else who has contributed to researching the original LU packets and making that information available to everybody. Without this knowledge, these projects wouldn't exist.
* Lego, for creating a game that was amazing and died too soon.
* Everybody who has contributed to this project and helped keep LU alive!  