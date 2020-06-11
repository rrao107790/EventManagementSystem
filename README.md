# EventManagementSystem
Data Driven Web Application

Design and implement a Web based event management system.

· Events have title, start date and optionally start time. Events may have also (optionally) duration, description, location and author. Events can be public (visible by everyone) and private (visible to their owner author only). Events may have comments. Comments belong to certain event and have content (text), date and optional author (owner).

· Anonymous users (without login) can view all public events. The home page displays all public events, in two groups: upcoming and passed. Events are shown in short form (title, date, duration, author and location) and have a [View Details] button, which loads dynamically (by AJAX) their description, comments and [Edit] / [Delete] buttons.

· Anonymous users can register in the system and login / logout. Users should have mandatory email, password and full name. User’s email should be unique. User’s password should be non-empty but can be just one character.

· Logged-in users should be able to view their own events, to create new events, edit their own events and delete their own events. Deleting events requires a confirmation. Implement client-side and sever-sidevalidation data validation.

· A special user “admin@admin.com” should have the role “Administrator” and should have full permissions to edit / delete events and comments.
