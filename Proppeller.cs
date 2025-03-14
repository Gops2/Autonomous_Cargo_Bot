#include "simpletools.h" // Include simple tools
#include "servo.h" // Include Servo library
#include "ping.h" // Include Ultrasonic library
static volatile int
LS,RS,i,n,Lefty,Righty,y,an,bn,m,m_start,k,p,pick,obs_flag,int_flag,cog1,last_int
;

//Initialize all the variables
void Check_obs(); // Forward Declaring
functions
void package();
void led_obs();
serial *bm;
unsigned int stack[40+25]; // Defining Stacks
unsigned int stack1[40+25];
unsigned int stack2[40+25];
const int obs_led = 3; //Led for obstruction at pin 3

int main() // Main function
{
set_direction(obs_led,1); //LED to indicate
obstruction
cogstart(&led_obs,NULL,stack2,sizeof(stack2)); // Initialising COG, COG 1 &
COG 2
cogstart(&package,NULL,stack1, sizeof(stack1));
cogstart(&Check_obs,NULL,stack,sizeof(stack));
bm = serial_open(10,9,0,9600); // Connecting to serial port
dprint(bm,"Team 10\n"); // To indicate starting of
Robot
pause(200);
n=0; // Setting counters and flags
pick = 0;
obs_flag = 0;
last_int = 0;
while(1)
{

27

LS = input(12); // Assigning the left IR to
the port
RS = input(13); // Assigning the Right IR to
the port
Lefty = 16; // Assigning the Left SERVO
Motor to the port
Righty = 17; // Assigning the Right SERVO
Motor to the port
Forward(); // Calling the Forward
Movement Function
print("At forward\n");
if((LS == 1)&&(RS == 1)) // Initialising IF loop for
detecting intersection
{
n++; // Intersection counter in
the S lane
Check_obs(); // Calling the check Obstacle
function to look out for obstacles in the path
if (n == 5) // Reaching the end of S line
{
right(); // Calling the right turn
function to switch towards A lane
Forward(); // Calling the forward
movement function
right(); // Calling the right turn
function to allign orientation on the A line
int_flag = 1;
A_line(); // Calling the A line
function for performing task at A line
right(); // Calling right turn
function to switch to B line
Forward();
fw();
Forward(); // Moving forward
right(); // Turning right to allign
along the B line
B_line(); // Calling B line function to
perform tasks at B line
Stop(); // Stop the robot and display
distance
}
else if(k < 40) // Incase an obstacle is
detected on the S line
{
if(n<4)

28

{
obs_flag = 1;
cog_run(led_obs,128);
//dprint(bm,"Obstruction at S line\n");
obs_flag = 0;
left();
fw();
Forward();
right();
fw();
Slow_forward_D(); // Code for using alternate B

line at a reduced speed and coming back to the S line

dprint(bm,"Intersection\n");
right();
fw();
Forward();
n++;
p = ping_cm(14);
if(p<40) // After coming back checking

for another obstacle on S line and again detouring via B line

{
obs_flag = 1;
pure_180();
obs_flag = 0;
Forward();
right();
Slow_forward_D();
obs_flag = 0;
dprint(bm,"Intersection\n");
right();
Forward();
n++;
if (n == 5) //Checking for end of S line
{
fw();
Forward(); //

Travelling to the A line directly from B line

right(); //

Alligning the robot with the A line

cog1 = cogstart(&Check_obs,NULL,stack, sizeof(stack)); //

Starting cog1 front ultrasonic for obstruction detection

int_flag = 1;
A_line(); //

Calling A line function for performing A line task

right();

29

Forward();
;
fw();
Forward();
;
right();
B_line(); //

Calling B line function for performing B line task

Stop(); //

Stopping the Robot
}
else
{
left(); // Alligning the robot to S

line when it returns from B line after detour

}
}
else
{
left(); // Alligning the robot to S

line when it returns from B line after detour

}
}
else if(n == 4) // If Obstruction detected at
4th intersection
{
obs_flag = 1;
cog_run(led_obs,128); // cog for detecting

obstruction in front

dprint(bm,"Obstruction at S line\n");
obs_flag = 0;
left(); // Detouring via B line
fw();
Forward();
right();
fw();
Slow_forward_D();
obs_flag = 1;
dprint(bm,"Intersection\n");
right();
fw();
Forward();
n++; // Directly

going towards the A line

fw();

30

Forward();
right(); // Alligning

the robot with A line

cog1 = cogstart(&Check_obs,NULL,stack, sizeof(stack)); //

Initialising cog
int_flag = 1;
A_line(); // Calling A

line function for the robot to perform its certain tasks on the A
line

right(); // Turning on

the vertical line after reaching the end of A line

Forward(); // Moving

along the vertial line from A line to B line after picking up the
widget/cargo/object
;
fw();
Forward();
;
right(); // Alligining

the robot with B line

B_line(); // Calling B

line function for the robot to perform its certain tasks on the B line

Stop(); // Stopping

the Robot
}
}
else
{
fw(); // Moving
forward on the S line normally
}
}
}
}
void A_line() // The A line Function
{
an = 1; // Initialising the flags and
counters
m = 0;
m_start = 0;
pick = 0;
p = ping_cm(14);
int_flag = 0;

31

if(p<20) // Checking for cargo/widget
to be picked up at turning
{
Stay(); // Pause for 2 seconds If
widget is detected by sensor
dprint(bm,"Widget picked up\n"); // Pick up being indicated by
LED and displayed via the bluetooth module connected display
high(26);
pause(1000);
low(26);
pause(1000);
pick++; // Flag to indicate pick up
}
while(1)
{
LS=input(13); // Left IR being assigned to
port 13
RS=input(12); // Right IR being assigned to
port 12
Lefty = 16; // Left servo being assigned
to port 16
Righty =17; // Right servo being assigned
to port 17
k = ping_cm(15); // Front side ultrasonic
sensor being assined port 15 and initialised (for widget)
p = ping_cm(14); // Left side ultrasonic
sensor being assined port 14 and initialised (for obstruction)
if(k>10) // If obstruction not
detected
{
if ((LS == 1) && (RS == 1)) // Intersection being reached
{
an++; // Intersection counter
dprint(bm, "Intersection\n");
if(pick >0)
{
m++; // Distance counter
}
if(p<20) // Widget detected
{
if(pick == 0)
{
Stay(); // Pauses

32

dprint(bm,"Widget picked up\n"); // Imitating widget pick up
high(26); // Pick up being indicated by

LED blink as well

pause(1000);
low(26);
pause(1000);
pick++;
}
}
if(an == 5) // End of A line reached
{
break; // Getting out of A line

function
}
else
{
Slow_fw(); // Moving normally on A line
}
}
else if ((LS == 0) && (RS == 1)) // Line correction
{
obs_flag = 0;
Slow_correction_l();
}
else if ((LS == 1) && (RS == 0))
{
obs_flag = 0;
Slow_correction_r();
}
else if ((LS == 0) && (RS == 0))
{
obs_flag = 0;
Slow_straight();
}
}
else // Else if obstacle present
then stop and wait for the obstacle to be removed
{
obs_flag = 1; // Obstacle detected
Stay();
}
}
}

33

void B_line() // Initialising the A line
Function
{
bn = 1;
p = ping_cm(14); // Left side Ultrasonic
Sensor being assigned to port 14 and initialised
if(p<20) // If widget detected by left
side ultrasonic sensor
{
dprint(bm,"Widget dropped\n"); // Imitating Widget dropping
Stay();
high(27); // LED blinking to indicate
pause(1000);
low(27);
pause(1000);
char str[10];
dprint(bm, "The distance between the pickup and drop off is\n");
int z = m*40; // Calculating the total
distance
sprintf(str, "%d", z); //Converting int to string
for bluetooth connection exchange
dprint(bm,str); // Displaying the distance
travelled between pickup and drop off point
Stop(); // Stop the Robot
}
while(1)
{
LS=input(13); // Left IR being assigned to
port 13
RS=input(12); // Right IR being assigned to
port 12
Lefty = 16; // Left servo being assigned
to port 16
Righty =17; // Right servo being assigned
to port 17
k = ping_cm(15); // Front side ultrasonic
sensor being assined port 15 and initialised (for widget)
p = ping_cm(14); // Left side ultrasonic
sensor being assined port 14 and initialised (for obstruction)
if(k>10) // If obstruction not
detected
{
if ((LS == 1) && (RS == 1)) // Intersection being reached

34

{
bn++; // Intersection counter
m++; // Distance counter
;
dprint(bm,"Intersection\n"); // Displaying intersection
if(p<20) // Detecting widget(Drop off

point) case on B line

{
{
dprint(bm,"Widget dropped\n"); // Imitating dropping the

widget

Stay();
high(27); // Widget drop off indicated

by blinking of LED
pause(1000);
low(27);
pause(1000);
char str[10];
dprint(bm, "The distance between the pickup and drop off is\n");
sprintf(str, "%d", m*40);
dprint(bm,str); //Displaying the distance

travelled between pickup and drop off point

Stop(); // Stop the Robot
}
}
if(bn == 5) // reaching the end of B line
{
break;
}
else
{
Slow_fw();
}
}
else if ((LS == 0) && (RS == 1)) // Line following correction
{
obs_flag = 0;
Slow_correction_l();
}
else if ((LS == 1) && (RS == 0))
{
obs_flag = 0;
Slow_correction_r();
}

35
else if ((LS == 0) && (RS == 0))
{
obs_flag = 0;
Slow_straight();
}
}
else
{
obs_flag = 1; // If obstruction detected on
B line the robot pauses and waits for the obstacle to be removed
Stay();
}
}
}

void Stop() // Function to permanently
stop the robot
{
while(1)
{
servo_speed(Lefty,0);
servo_speed(Righty,0);
}
}
void Stay() // Function to pause the
robot for a while
{
servo_speed(Lefty,0);
servo_speed(Righty,0);
}
void straight() // Function to move the robot
straight
{
servo_speed(Lefty,50);
servo_speed(Righty,-50);
}
void Forward() // Function to move the robot
forward with path correction
{

36

while(1)
{
LS = input(12);
RS = input(13);
Lefty = 16;
Righty = 17;
if((LS == 1)&&(RS == 1))
{
dprint(bm,"Intersection\n");
if(pick >0)
{
m++;
}
break;
}
else if((LS == 0)&&(RS == 1))
{
correction_r();
}
else if((LS == 1)&&(RS == 0))
{
correction_l();
}
else if((LS == 0)&&(RS == 0))
{
straight();
}
}
}

void pure_180() // Function to turn the
robot by 180 degrees
{
for(i = 0;i<1450;i++)
{
servo_speed(Lefty,-50);
servo_speed(Righty,-50);
}
}

void correction_l() // Function to correct the
robot path tilted towards left
{

37

servo_speed(Lefty,50);
servo_speed(Righty,0);
}
void correction_r() // Function to correct the
robot path tilted towards right
{
servo_speed(Lefty,0);
servo_speed(Righty,-50);
}
void Slow_forward() // Function to move the
robot forward SLOWLY with path correction
{
while(1)
{
LS = input(12);
RS = input(13);
Lefty = 16;
Righty = 17;
if((LS == 1)&&(RS == 1))
{
//dprint(bm,"Intersection\n");
break;
}
else if((LS == 0)&&(RS == 1))
{
Slow_correction_r();
}
else if((LS == 1)&&(RS == 0))
{
Slow_correction_l();
}
else if((LS == 0)&&(RS == 0))
{
Slow_straight();
}
}
}
void Slow_forward_D() // Function to move the
robot on B line during deviation from S line, forward with path correction
{
while(1)

38

{
LS = input(12);
RS = input(13);
Lefty = 16;
Righty = 17;
k = ping_cm(15);
if(k>10)
{
obs_flag = 0;
if((LS == 1)&&(RS == 1))
{
//dprint(bm,"Intersection\n");
break;
}
else if((LS == 0)&&(RS == 1))
{
Slow_correction_r();
}
else if((LS == 1)&&(RS == 0))
{
Slow_correction_l();
}
else if((LS == 0)&&(RS == 0))
{
Slow_straight();
}
}
else if(k<11)
{
obs_flag = 1;
Stay();
}
}
}

void Slow_correction_l() // Function to correct the
robot path tilted towards left when travelling slowly
{
servo_speed(Lefty,20);
servo_speed(Righty,0);
}
void Slow_correction_r() // Function to correct the
robot path tilted towards right when travelling slowly

39

{
servo_speed(Lefty,0);
servo_speed(Righty,-20);
}
void Slow_straight() // Function to move forward
slowly
{
servo_speed(Lefty,20);
servo_speed(Righty,-20);
}
void right() // Function to turn right
{
for(i=0;i<350;i++)
{
straight();
}
for(i=0;i<700;i++)
{
servo_speed(Lefty,50);
servo_speed(Righty,50);
}
}
void left() // Function to turn right
{
for(i=0;i<350;i++)
{
straight();
}
for(i=0;i<700;i++)
{
servo_speed(Lefty,-50);
servo_speed(Righty,-50);
}
}
void Check_obs() // Function to check for
obstruction in path
{
k = ping_cm(15);
}

40

void fw() // Function to move forward
{
for(i=0;i<200;i++)
{
straight();
}
}
void Slow_fw() // Function to move forward slowly
{
for(i=0;i<400;i++)
{
Slow_straight();
}
}
void package() // Function for left ultrasonic if
the robot is at end of line
{
if(n==5)
{
int p = ping_cm(14);
}
}
void led_obs() // Function to light up LED when
obstruction is faced
{
while(1)
{
if(obs_flag == 1)
{
high(obs_led);
pause(100);
if(n<5)
{
pause(400);
}
}
else if(int_flag == 1)
{
low(obs_led);
pause(300);
}


else
{
low(obs_led);
}
}
}