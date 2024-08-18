

### Objective

Develop an automated guided vehicle (AGV) system designed to autonomously navigate a factory floor, efficiently picking up and delivering widgets while adhering to lane protocols and avoiding obstacles using the Parallax Propeller.

### Operational Context

The AGV operates within a simulated grid-like factory layout. Starting from a designated home base, the vehicle follows specific lanes to retrieve widgets from stations in lane A and delivers them to stations in lane B for further processing. The system is engineered to recognize intersections, adjust for obstacles, and interact seamlessly with pickup and drop-off points.

### Functional Requirements

- **Guided Navigation:** Follow black tape lines to maintain accurate course alignment.
- **Lane Prioritization:** Traverse the central lane before accessing pickup or drop-off lanes.
- **Intersection Handling:** Detect intersections and provide real-time signaling without stopping.
- **Obstacle Avoidance:** Dynamically reroute to bypass obstacles in the central lane.
- **Speed Regulation:** Reduce speed when entering pickup and drop-off lanes.
- **Pickup Detection:** Identify objects at pickup points in lane A and pause momentarily for loading.
- **Adaptive Continuation:** Bypass empty pickup locations and continue without stopping.
- **Distance Calculation:** Measure and display the distance between pickup and drop-off points.
- **Dynamic Monitoring:** Continuously check for and respond to obstacles in pickup and drop-off lanes.


![Circuit Diagram](https://github.com/user-attachments/assets/b66472f0-fd09-4196-85fd-4360ff9a6fdf)


