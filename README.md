# Operator 4.0 - HoloLens 2 Application
Application for the Supervision and Monitoring System, developed in Unity (version 2019.4.2f1). 

In other to successfully record the movement which will be reproduced by the robot later, some steps have to be followed:
- First, the user has to choose the robot that wants to program. For the purpose of this case study, there were only introduced two robots: ABB IRB 2600 (*ABB*) and Universal Robots UR5 (*UR5*).
- Secondly, the user has to define the coordinate system, so that it matches the robot's referential. This definition was optimized as much as possible so it would be simpler for the user. In that sense, the coordinate system definition is made with just two points.
- After the coordinate system is define, the application draws the robot workspace, accordingly to the characteristics of that specific robot. The idea is that the user only records movements that the robot can reproduce.
- At this point the user can record the movement with his/her right index finger tip. While the user is recording the movement, a hot pink line marks its path to clarify what is being recorded.
- Finally, when the movement is finalized, the user can confirm that the movement was correctly recorded and it is sent to the robot. Otherwise, the movement is erased and the user has to start the recording again. The tool used to make the connection between HoloLens 2 and ROS was ROS\#.


![app_overview](https://user-images.githubusercontent.com/76999213/120467259-e686bd00-c397-11eb-8175-8df0bba689a0.png)

## Author
Inês de Oliveira Soares (ines.o.soares@inesctec.pt | up201606615@up.pt)
- Master Student - Electrical and Computer Engineering @ FEUP
- Master Thesis Development @ INESC TEC
