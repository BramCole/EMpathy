# Architecture and Design Document


## 1. Considerations


#### 1.1 Assumptions
*In this section describe any assumptions, background, or dependencies of the software, its use, the operational environment, or significant project issues.*

#### 1.2 Constraints
*In this section describe any constraints on the system that have a significant impact on the design of the system.*

#### 1.3 System Environment
*In this section describe the system environment on which the software will be executing. Include any specific reasons why this system was chosen and if there are any plans to include new sections to the list of current ones.*

## 2. Architecture


#### 2.1 Overview
*Provide here a descriptive overview of the software/system/application architecture.*

#### 2.2 Component Diagrams
*Provide here the diagram and a detailed description of its most valuable parts. There may be multiple diagrams. Include a description for each diagram. Subsections can be used to list components and their descriptions.*

![image](https://user-images.githubusercontent.com/46005397/110365049-8b506300-8012-11eb-8985-1119d4687f47.png)


#### 2.3 Class Diagrams
*Provide here any class diagrams needed to illustrate the application. These can be ordered by which component they construct or contribute to. If there is any ambiguity in the diagram or if any piece needs more description provide it here as well in a subsection.*

![image](https://user-images.githubusercontent.com/46005397/110364465-cef69d00-8011-11eb-8a77-f7edb1740c11.png)


#### 2.4 Sequence Diagrams
*Provide here any sequence diagrams. If possible list the use case they contribute to or solve. Provide descriptions if possible.*

#### 2.5 Deployment Diagrams
*Provide here the deployment diagram for the system including any information needed to describe it. Also, include any information needed to describe future scaling of the system.*


#### 2.5.1 deployment For Personal Use and or Use Testing
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


#### 2.6 Other Diagrams
*Provide here any additional diagrams and their descriptions in subsections.*

## 3 User Interface Design
*Provide here any user interface mock-ups or templates. Include explanations to describe the screen flow or progression.*

## 4 Appendices and References


#### 4.1 Definitions and Abbreviations
*List here any definitions or abbreviations that could be used to help a new team member understand any jargon that is frequently referenced in the design document.*

#### 4.2 References
*List here any references that can be used to give extra information on a topic found in the design document. These references can be referred to using superscript in the rest of the document.*
