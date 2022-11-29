# Data Tracking Application for Evans HS Cross Country Team
Created by Haley Rohe and Ricardo Price for CIST2932 Advanced Programming Topics
---
## Introduction
This application is intended to help the Cross Country team keep track of their run data. The app allows the team to record the attendance of runners at practices, record the runners' distance and run time for each practice, view run data for past practices, make and respond to announcements, and calculate run paces based on distance and run time. Users of the application fall into five categories: Master Admin, Coach, Runner, Waiting for approval, and Banned user. 
- The Master Admin user(s) have full access priviledges to all areas of the app. 
- The Coach users have access to most areas of the app, excluding the Master Admin panel.
- The Runner users have limited access to only the pages associated with their own data, and limited access to the forum.
- The Waiting For Approval and Banned users cannot access the application.
All new registered users will automatically be assigned to the Waiting For Approval category. 
## Getting Started
To get started, log in to the Master Admin account using your given credentials. (These instructions will also apply for Coach accounts.) Once you're logged in, there are several options in the navigation bar or menu to choose from. For this Getting Started guide, you will walk through the process of starting a new practice and recording the attendance of runners, scheduling a practice for later, assigning a workout to a practice, signing a Runner out of practice, and ending the practice. However before you can use these features, you will need to register and approve 2-3 sample Runner accounts to populate the fields and tables required for these activities.    
  To start a new practice, first click the Practices tab in the navigation bar to pull up the Practice options, and click Start Practice Now. Once you are on the Start a New Practice Session page, you can enter a location name for the practice, as well as mark Runners as present (for this tutorial, mark at least one Runner as present/attending). Once you have entered the approporiate data, click the Start Practice button. If the practice was started successfully, you will see an alert confirming the entry.    
  Next, you will schedule a practice session for later. From the navigation bar, click the Practices option, then Schedule Practice for Later. Once you are on the Schedule a Practice Session for Later page, you can enter a location name, a start and end time, and mark Runners as attending if desired. Once you have entered the approporiate data, click the Start Practice button. If the practice was started successfully, you will see an alert confirming the entry.    
  Now you will assign a new workout to the practice session you just started. From the navigation bar, click the Workouts tab in the navigation bar, then choose the Assign Workouts option. On that page, you will see a table with the practice sessions you previously created. In this table, select the most current practice and click the Assign Workouts button. This will take you to the page where you can see the Runners attending the practice and assign workout types to their individual practice sessions. Once you are finished assigning workouts, click the submit button. After submitting, the application will take you back to the Assign Workouts page. To view the workouts you assigned the each Runner, click the Workouts tab in the navigation bar, then choose Athelete Workout History. This will take you to the page where you can select which Runner to view history for. Click the View button in the Past Practices column to select the Athelete whose history you want to see, and this will take you to the page where you can view all the past practices and workout types for that athelete.
  Next, you will sign a Runner out of practice. Click the Practices tab in the navigation bar and choose the Practices In Progress option. You should see the practices you started / scheduled listed. To sign a Runner out of practice, click the button in the View Current Attendance column in the row of the practice you started. You will be taken to a page where you can see the Runners signed into the practice and a button to sign each Runner out of the practice. Click the Sign Out button in the row of the Runner you want to sign out. Once the Runner has been signed out you will be taken back to the Practices In Progress page.
  Finally, you will end the practice you just signed a Runner out of. On the same Practice In Progress page, click the button under the End Practice row. The application will ask you to confirm that you want to end that specific practice. Click the button to confirm your choice. An alert will pop up confirm the successful end of that practice.

## Forums
The Forums page allows Coach and Master Admin accounts to create and delete threads as well as respond to threads via comments. Runner accounts may only comment on threads. 
## Calculators
The application comes with three different pace calculators:
1. Simple Pace Calculator
2. Jack Daniels VDOT running calculator
3. Omni Pace Calculator
### Simple Pace Calculator
The Simple Pace calculator takes in values for distance and time and calculates and displays the running pace. To use this calculator, enter a numerical value in the Distance field, then enter numerical values for the Time fields in the format of Hours, Minutes, Seconds. Click the 'Calculate' button to calculate the pace, which is displayed in the 'Your Pace:' field. To clear all values in the calculator, click the 'Clear' button.
### Jack Daniels VDOT Running Calculator
The embedded Jack Daniels VDOT running calculator and its instructions are sourced from from the Run S.M.A.R.T. Project (https://runsmartproject.com). 

How To Use The Calculator
1. Find your goal race pace:
  - Select distance and input your goal time
2. Find out how fast you should train based on a recent performance
  - Select distance and input your time from a recent race
  - Click the “Training Paces” tab to learn how fast you should be running different workouts
  - Click on the workout types for an explanation
3. Find out what the equivalent performances are for other distances based on a race result
  - Select distance and input time
  - Click the Equivalents tab
4. Find out how much conditions like wind, temperature or altitude had on your performance or will have on an upcoming workout or future race
  - If you have an upcoming race or workout and want to know how wind, temperature or altitude will affect the performance:
    1. Add in a race result or a race performance that represents what type of shape you are in
    2. Select “Anticipate” and then enter the condition
    3. Click calculate to find out what the effect will be
  - If you’ve run a race and want to find out what effect wind, temperature or altitude had on your performance:
    1. Input your race result
    2. Select “Result” and then enter the condition
    3. Click calculate to find out how much the condition hurt your performance
### Omni Pace Calculator
To calculate your different training paces, enter a numerical value in the Distance field, then enter values for hours, minutes, and seconds in the Your time field. The calculator automatically displays your training paces based on the values you enter.
