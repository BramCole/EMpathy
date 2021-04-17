# Requirements Document



Highest level: iOS app with UI for selecting between lessons/demonstrations that have been prepared. 
Lesson/demonstration selection: accesses the AR environment which we have prepared using Vuforia. Access to the user's smartphone camera at this step. Tracking of actual objects/surfaces not necessary? 
AR environment: will have objects generated and placed using Unity per the physics computation. Unity links object placement/display to back-end physics computation. Unity-based interface to selct whether to show field lines, potential lines, etc. at this level?
Physics computation: Lesson plans will select between scripts prepared to position charges etc., and to calculate the position of field lines. Complexity of simulation we are able to achieve at this level will determine the lessons we make available. 





### 1. User Requirements


##### 1.1 User Characteristics  
*Provide here the characteristics of a typical user of the system (target audience). List any technical background or expected prior experience.*
Students: Easy to use, minimal technical background required. Should understand basic ideas like vectors, and some basic knowledge of how charges interact. Basic functionality shouldn't require knowledge past basic smartphone operation.

Admin: Permission to modify the database, total technical access. (Database management, power to create and revoke/modify user permissions etc)

##### 1.2 System's Functionality  
*Provide here an overview of the system and what the overall intention of the system is. (goal, who is this for, ect)* 
Interface to deliever E&M lessons through the visualization of E&M fields. Primarily designed for students
      
##### 1.3 User Interfaces   
*Provide here a brief description of how the user will interact with the system.*
User interacts through touch. Will drag and drop models (Point charges, wires, charged plates). User can also move the camera to reposition themselves in the enviroment.

### 2. System Requirements  
*Requirements should be listed as user stories: As a <role> I can <capability>, so that <receive benefit>.*

##### 2.1 Functional Requirements
*List here the functional requirements of the system. Functional requirements are requirements that specify __what__ the system should do and can be thought of as 'the system must do <requirement\>'. Implementation details for each requirement should be addressed in the system design document. An example of a functional requirement would be 'the system utilizes Java version...' This list can become quite extensive and for best practice each requirement should be issued its own unique name, number, and be accompanied by a description.*

* As a student I can move my camera around so I can navigate the world in 3D
* As a student I can drag and drop charged objects to visualize their E&M fields
* As a student I'm able to view the Electric field from a charged object
* As a student I'm able to view the Magnetic field from a charged object
* As a student I'm able to view the Electric displacement field from a charged object
* As a student I'm able to view the H field from a charged object
* As a student I'm able to create constant fields in the enviroment without placing sources
* As a student I'm able to engage with premade lessons to learn basic E&M ideas with guidance
* As a student I'm able to visualize vector calculus operations (dot product, cross product,divergence, curl, gradients, addition, subtraction etc)
* As a student I can modify the charges of objects in the enviroment
* As a student I can modify the positions of objects in the enviroment
* As a student I can view the dynamic effects of fields on moving charges
* As a student I'm able to access a sandbox mode where I can experiment with the application without a lesson plan or restrictions


#####2.2 Non-Functional Requirements
*List here the non-functional requirements of the system. Non-Functional requirements are requirements that specify __how__ the system should act and can be thought of as 'the system shall be <requirement\>'. An example of a non-functional requirement would be 'the system input should be able to handle any file smaller than...'*

* As a student I can go from opening the app to basic functionality (placing field objects) within 3 minutes
* As a student I can expect field lines to be calculated in real time for objects that I place
* As a student I can expect the application to crash very infrequently (less than 1% of the time with typical use)
* As a student with minimal technical experience and no app experience I should be able to access app features within 15 minutes (shallow learning curve)
* As a student I should expect to be able to complete and understand a lesson in 30 minutes
* As a programmer I should be able to easily read and understand the application's documentation
* As a tester I can expect the code base to be modular, cohesive and low coupling. (Not spaghetti code)


### 3. Not Doing

#### 3.1 Not Doing List
*list things that are out of scope or will be done at a later date*
