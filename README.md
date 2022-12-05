# Data Tracking Application for Evans HS Cross Country Team
Created for CIST2932 Advanced Programming Topics, Augusta Technical College, Fall 2022
---
## Contents  
 1. [Introduction](#introduction)  
 2. [Getting Started as a Main Admin](#getting-started)
 3. [Runner Accounts](#runner-accounts)
 4. [Coach Accounts](#coaches)  
 5. [Main Admin Accounts](#main-admin)
### Introduction
This application is intended to help the Cross Country team keep track of their run data using their phones and computers. The app allows the team to record the attendance of runners at practices, record the runners' distance and run time for each practice, view run data for past practices, make and respond to announcements, and calculate run paces based on distance and run time. Users of the application fall into five categories: Main Admin, Coach, Runner, Waiting for approval, and Banned user. 
- The Main Admin user(s) have full access priviledges to all areas of the app. 
- The Coach users have full access priviledges, except being able to switch other users' roles to Admin.
- The Runner users have limited access to only the pages associated with their own data, and limited access to the forum.
- The Waiting For Approval and Banned users cannot access the application.  
All new registered users will automatically be assigned to the Waiting For Approval category, where they will need to be approved by an Admin or Coach account. 
## Getting Started
 - To get started, log in to the Main Admin account using your given credentials. (These instructions will also apply for Coach accounts.) Once you're logged in, there are several options in the navigation bar or menu to choose from. For this Getting Started guide, you will walk through the process of starting a new practice and recording the attendance of runners, scheduling a practice for later, assigning a workout to a Runner, signing a Runner out of practice, and ending the practice. However before you can use these features, you will need to register and approve 2-3 sample Runner accounts to populate the fields and tables required for these activities.    
 - To start a new practice, first click the Practices tab in the navigation bar to pull up the Practice options, and click Start Practice Now. Once you are on the Start a New Practice Session page, you can enter a location name for the practice, as well as mark Runners as present (for this tutorial, mark at least one Runner as present/attending). Once you have entered the approporiate data, click the Start Practice button. If the practice was started successfully, you will see an alert confirming the entry, and you will see the practice session in the list of current practices on the Practices In Progress page.    
 - Next, you will schedule a practice session for later. From the navigation bar, click the Practices option, then Schedule Practice for Later. Once you are on the Schedule a Practice Session for Later page, you can enter a location name, a start and end time, and mark Runners as attending if desired. Once you have entered the approporiate data, click the Start Practice button. If the practice was started successfully, you will see an alert confirming the entry, and you will see the practice session in the list of current practices on the Practices In Progress page.    
 - Now you will assign a new workout to the practice session you just started. From the navigation bar, click the Workouts tab in the navigation bar, then choose the Assign Workouts option. On that page, you will see a table with the practice sessions you previously created. In this table, select the most current practice and click the Assign Workouts button. This will take you to the page where you can see the Runners attending the practice and assign workout types to their individual practice sessions. Once you are finished assigning workouts, click the submit button. After submitting, the application will take you back to the Assign Workouts page. To view the workouts you assigned the each Runner, click the Workouts tab in the navigation bar, then choose Athelete Workout History. This will take you to the page where you can select a Runner to view their workout history. Click the View button in the Past Practices column to select the Athelete whose history you want to see, and this will take you to the page where you can view all the past practices and workout types for that athelete.
 - Next, you will sign a Runner out of practice. Click the Practices tab in the navigation bar and choose the Practices In Progress option. You should see the practices you started / scheduled listed. To sign a Runner out of practice, click the button in the View Current Attendance column in the row of the practice you started. You will be taken to a page where you can see the Runners signed into the practice and a button to sign each Runner out of the practice. Click the Sign Out button in the row of the Runner you want to sign out. Once the Runner has been signed out you will be taken back to the Practices In Progress page.
 - Finally, you will end the practice you just signed a Runner out of. On the same Practice In Progress page, click the button under the End Practice row. The application will ask you to confirm that you want to end that specific practice. Click the button to confirm your choice. An alert will pop up confirm the successful end of that practice.   
## Runner accounts   
Runners can access three main areas of the application:
  1. Forums
  2. Practices
  3. Calculators   
### Forums
Runner accounts can comment on threads and reply to other comments, but may not create or delete threads.   
### Practices
The Practices tab contains two options: My Practices and My Workouts.  
 - The My Practices page displays all of the practices that are currently in progress or scheduled for a future date / time. The Runner has the option to join an open practice, then sign out of a practice once the practice is complete.  
 - The My Workouts page displays the practices that the Runner is signed into, that also have workouts assign to them by a Coach or Admin. Once their workout is assigned, the Runner can enter their run data for that workout.  
    - To enter your run data, click the Log Data button in the row of the practice you want to record data for. You will be taken to a page with input fields for the distance you ran and your run time in the format of hours, minutes, seconds.  
    - You can also edit your data after you log it, but only until while the practice is in session. If a Coach or Admin closes the practice, you will not be able to change your data.  
## Calculators  
The application comes with three different pace calculators:  
 1. Simple Pace Calculator  
 2. Jack Daniels VDOT running calculator  
 3. Omni Pace Calculator  
### Simple Pace Calculator
The Simple Pace calculator takes in values for distance and time and calculates and displays the running pace. To use this calculator, enter a numerical value in the Distance field, then enter numerical values for the Time fields in the format of Hours, Minutes, Seconds. Click the 'Calculate' button to calculate the pace, which is displayed in the 'Your Pace:' field. To clear all values in the calculator, click the 'Clear' button.   
### Jack Daniels VDOT Running Calculator
The embedded Jack Daniels VDOT running calculator and its instructions are sourced from from the Run S.M.A.R.T. Project (https://runsmartproject.com). 
 > How To Use The Calculator  
 > 1. Find your goal race pace:
 >   - Select distance and input your goal time
 > 2. Find out how fast you should train based on a recent performance
 >   - Select distance and input your time from a recent race
 >   - Click the “Training Paces” tab to learn how fast you should be running different workouts
 >   - Click on the workout types for an explanation
 > 3. Find out what the equivalent performances are for other distances based on a race result
 >   - Select distance and input time
 >   - Click the Equivalents tab
 > 4. Find out how much conditions like wind, temperature or altitude had on your performance or will have on an upcoming workout or future race
 >   - If you have an upcoming race or workout and want to know how wind, temperature or altitude will affect the performance:
 >     1. Add in a race result or a race performance that represents what type of shape you are in
 >     2. Select “Anticipate” and then enter the condition
 >     3. Click calculate to find out what the effect will be
 >   - If you’ve run a race and want to find out what effect wind, temperature or altitude had on your performance:
 >     1. Input your race result
 >     2. Select “Result” and then enter the condition
 >     3. Click calculate to find out how much the condition hurt your performance

### Omni Pace Calculator
To calculate your different training paces, enter a numerical value in the Distance field, then enter values for hours, minutes, and seconds in the Your time field. The calculator automatically displays your training paces based on the values you enter.   
## Coaches   
Coaches can access six areas of the application:  
 1. Admin 
 2. Forums
 3. Data
 4. Practices
 5. Workouts
 6. Calculators   
### Admin   
The Admin tab takes the user to the Administrator Panel, where a Coach or Main Admin user can approve or reject new users, change the role of a user, edit a user's information (Name, Username, and Password), and delete users from the application. This section will walk through how to do each of those actions.  
 - To approve or reject new users, click the See New Users... button under the heading of the Admin page and choose the appropriate option. If there are no new users, the app will display an alert. Otherwise, the Coach or Main Admin can select each new user they want to approve and click the Submit button. An alert will be displayed confirming the successful approval or rejection.  
 - To change the role of a user, for example changing a Runner account to a Coach account, click the Change Role button in the row of the user you want to change. The app will display a new page where you can select which role to assign the selected user, then click the Confirm button. An alert will be displayed confirming the successful role change.  
 - To edit a user's information, click the Edit User button in the row of the user you want to change. The app will display a page where you can choose to change the user's Name, Username, and/or Password. Each button will display a form, where you can change the appropriate user information. To cancel the changes before submitting, click the Cancel button. Otherwise, click Submit. An alert will be displayed confirming the successful change.
 - To delete a user from the application, click the Delete button in the row of the user you want to delete. You will be asked to confirm the user you want to delete. All data for this user will be permanently deleted.  
### Forums  
Coaches can create and delete threads in the forum, as well as viewing, creating and deleting comments on the main thread.  
 - To create a new thread, click the 'Create new thread' button at the top of the page. The app will display a page with a form with fields for the title and body of the thread. Once you are satisfied with your new thread, click the Submit button to save your entry. The application will redirect to the Forum home page, where you will see your new thread at the top of the list.  
 - To delete a thread, click the Delete button under the body of the the thread you want to delete. You will be asked to confirm the thread you want to delete. The thread will be deleted permanently.
 - Viewing the main thread allows the user to view the replies to the thread. Click the Delete button under the reply to permanently delete it.   
   - Main thread replies can have their own replies. These replies are all deleted if the main reply is deleted.  
### Data
The pages in Data tab allow you to add, edit and delete workout data for Runners, as well as view statistics for Runners, and export data to a external spreadsheet. The tab contains the following options:  
  1. Practice Data  
  2. Runner Data  
  3. Statistics  
  4. Data Exportaion  
- The Practice Data page shows you the current practices that are in progress, with the option to switch views to practices that have ended. Select a practice to view the Runners who were present at that practice. Once you select a Runner, you can chose to Add, Delete, or Edit workout data for that Runner.  
- The Runner Data page shows a list of all Runner accounts, from which the user can select a Runner to view a list of all practices the Runner has attended. From there the user can Add, Delete or Edit workout data for the Runner's selected practice session.  
### Practices  
The pages in the Practices tab allows you to start practices immediately, schedule practices for later, and view and edit attendance records for practice sessions. The tab contains the following options:  
  1. Start Practice Now  
  2. Schedule Practice for Later  
  3. Practices In Progress  
  4. Closed Practices
  5. Athlete Practice History
 - The Start Practice Now and Schedule Practice for Later pages allow you to create practice sessions. The process to do so is covered in the [Getting Started guide](#getting-started).  
 - The Practice In Progress page displays a list of all the open practice sessions, as well as options to see the current attendance list and end each practice. 
  - To edit the attendance records for a practice session, click the button under View Current Attendance in the row of the practice you want to select. If there are no runners signed in to the practice session, you will have the option to add Runners to the practice manually by following the link displayed on the page. Otherwise, you will see a list of all Runners signed in to this practice session. You have the option to add a Runner to this practice session or sign out a Runner who is currently in the practice.
  - To end a practice session, click the button under End Practice in the row of the practice you want to select. You will be asked to confirm your choice. You cannot reopen a practice session once it has ended.  
 - The Closed Practices page displays a list of all practice sessions that have ended. To view the attendance and workout records for that practice, click the Select button in the row of the practice you want to view. You will be taken to a page where you can see the Runner Name, Location, Start and End times, and Workouts of each Runner present at that practice session.  
 - The Athlete Practice History page displays a list of all Runners with the option to view a list of the selected Runner's past practice sessions. Simply click the View button in the row of the Runner whose history you want to view.  
### Workouts  
The pages in the Workouts tab allow you to assign workouts to a Runner in an open practice and add/edit the workout types.  
 - To assign one or more workouts to a Runner, the Runner must first be signed into an open practic session. The Assign Workouts page displays a list of open practices with Runner who do not have workouts assigned to them. Select the practice you want to assign workouts to by clicking the button under Assign Workouts in the row of the practice session. You will be taken to a page where you can assign workout types to individual Runners who are signed into the selected practice session.  
 - To add a new workout type or edit the existing types, go to the Edit Workout Types page under the Workouts tab. On this page is displayed a list of workout types. You can select an existing workout type to change the name of the workout by clicking the button under Edit Workout Name in the appropriate row, or add an entirely new workout type by clicking the New workout button at the top of the page.  
### Calculators  
Refer to the [Calculators section](#calculators) under Runner accounts for instructions on how to use the provided calculators. 
## Main Admin  
The Main Admin account has all of the abilities and priviledges of a Coach account. In addition, Main Admin accounts can change other accounts into Main Admin accounts. To do this, navigate to the Administrator Panel by clicking the Admin tab, then click the Change Role button in the row of the user whose account you want to modify. On the following page you can then choose to change the role to Master Admin and click confirm. An alert will pop up confirming the successful role change. 
