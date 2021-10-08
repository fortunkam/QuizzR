# QuizExperiment

## Problem statement

The are 2 use cases for the site (possible 2 sites/applications)

In the Admin site
Adminstrator creates quiz
Quiz consists of a number of questions of the following types

- 4 possible answers, choose the correct one (MVP)
- true or false
- free text answer

Quiz questions have a text description (question) and an optional image.  If an image isn't provided a default one will be used. 
The quiz question will have one or more correct answers 
The quiz will have a timespan within which users need to provide an answer

The Adminstrator can then start a live quiz.  
The live quiz will be represented by a random number (live quiz id)
The adminstrator will control moving to each question.
Each live quiz question will display the question, image, options and a timer that will countdown.  
When the timer reaches zero the user responses will be collated (together with the time it took them to respond)
User will be assigned a score for the question based on correct answer + time to respond.
Leaderboard will be displayed with a prompt for the next question.
At the end of the questions a leaderboard will be displayed.
The adminstrator will then have the option to drop back to the editor.

User site
User will be prompted to enter a live quiz id
User will be prompted for a nickname (content filters?)
User will be placed in a wait state until the admin starts the quiz
When quiz starts, user will see the options for each quiz question and the timer
After each question the user will see their score and wait for the admin to move to the next question
At the end of the quiz the user will see their place in the leaderboard (synced with the admin display) 


