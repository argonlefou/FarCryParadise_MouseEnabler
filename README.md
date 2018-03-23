# Far Cry Paradise - Mouse Enabler

This arcade game is nativelly working only with Xinput devices (no mouse support).  
This small tool will install and simulate a virtual X360 Gamepad and feed the game with needed data so that a mouse can be used.  
This is just a Proof Of Concept application, and absolutelly not meant to be a final / frontend friendly application.  


## Installation

Please follow theses steps to make everything work :

**1)Microsoft X360 Controller driver**
If you are running Windows 7 or older, you will need to install official [Microsoft X360 Controller driver](https://www.microsoft.com/accessories/fr-fr/d/xbox-360-controller-for-windows).  

**2) Virtual X360 bus driver**
Then, to be able to emulate some X360 controller, you will need to install ScpVbus driver.  
As there are a few different drivers available, the safest way is to download and install [Touchmote](http://touchmote.net/) (be sure too chosse X86 or X64 according to your system). This will install a fully compatible ScpVbus driver.  


## How to use :

You can use the application by either running it from the file explorer or by command line.  

1a) If you are running it from windows explorer, you will have the choice to check a couple of checkbox to invert X and/or Y axis as desired.  

**_OR_**

1b) If you are running it from a command line, you can add some arguments to automaticaly invert axis as needed :
`FarCry_MouseEnabler.exe -invertx -inverty`

2) Run the game. If everything is Okay you'll see a new X360 device on your Windows devices. You have to let this program running while you're playing


## Controls

<table>
  <tr>
    <td colspan="4" align="center"><b>Far Cry Paradise Controls</b>
  </tr>  
  <tr>
    <td><b>Mouse Buttons</b></td>
    <td align="center">Left</td>
    <td align="center">Middle</td>
    <td align="center">Right</td>
  </tr>  
<tr>
     <td><b>Action</b></td>
    <td align="center"><i>Shoot</i></td>
    <td align="center"><i>Grenade</i></td>
    <td align="center"><i>Grenade</i></td>
  </tr>  
</table>