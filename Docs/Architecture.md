
### Deployment

#### For Personal Use and or Use Testing
<p>
  
  * Need an apple developer account for XCode. Need a Vuforia licence key.
  
  * To get a licence key go to Vuforia develop and create a new licence key. Add this key to the AR camera. If you want to walk around, you must go to AR camera play settings and  switch to a webcam you have or the simulator.
  
  * Go to file build settings. A bundle identifier should be set if not set the company name to EMWare and the product name to EMPathy. The bundle identifier should change to com.EMWare.EMPathy   if not change the identifier to this (since we are deploying on personal phones it doesn’t actually matter what bundle identifier you use). 
A camera usage description must be set in the player settings under configurations change it to ```$(PRODUCT_NAME) camera use ```

  * Vuforia also wants a minimum iOS version of 11.0 so change that in the player settings also. Once done build and run for ios

  * Once in XCode there will be a little window in the left that shows files. Click the blueprint one that probably says unity-iPhone. This will bring you to XCode project settings. Go to signing and check automatically manage signing. After choose a team. 

  * To connect your phone you must enable a developer mode both on Xcode and on IOS. **Xcode and ios usually need to be updated.** If there are problems make sure to read the errors. There are multiple debugging consoles in Xcode(separate one for your phone) so make sure you know where to find them.

  * This [tutorial video](https://www.youtube.com/watch?v=dwjziS3Jjmk) is helpful if you are having problems exporting from unity to Xcode. 
</p>
