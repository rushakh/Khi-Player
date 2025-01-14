  ![Khi Player Icon](https://github.com/rushakh/Khi-Player/assets/151368929/d9e80b24-1ad5-4d07-bd88-8b4a06b20045)

Edit:

-Improved performance and stability

-Addressed memory leaks and removed unnecessary usage of IDisposable interface and many instances of forced collection of Garbage Collector that, due to their nature as stop-gaps, were slowing down the program

-Reworked and restructured the classes, and separated them from the main program

-Separated  the UI functionalities and data processes even further 

-Many of the functions now work asynchronously  

-Retouched code formatting, removed unnecessary spaces, and improved readability

-Improved the Seek bar's functionality and improved it's performance and accuracy

-Improved and Debugged codes related to playback

-Some problems with the UI's resistibility were addressed 

---

How it Began:

This is not actually the first version, more like the 20-something-th version but it is the first that I'm sharing hence v1. 

I started this as I was learning how to read and write txt files using code which turned into a simple idea for a pseudo-database (sort of a long shot but I'll go with that for now); as I changed different parts and learned new things, thanks to the frustration that writing this was causing me, I'd go back and rewrite the older parts. 

For example, the database was at first a simple txt file with the path of the files, then I included more details and shaped it up but then I hit a wall; I had trouble differentiating files from each other so I came up with an idea to use sth like "|*" and "*|" for every piece of info that I was using, it made some things easier but other stuff harder in addition to producing other sorts of problems. 

Long story short, I discovered XML and used it as a sort of database and stuck with it. There are many aspects that still need improving, especially regarding features, making use of async and more importantly, regarding errors since I did not use try catch in most parts but tried to eliminate their production and then did not have much time to go back and do it.

Anyhow, I'm happy with how it turned out, and I'll of course update, debug and develop it as I go on but since I'm short on free time and have to dedicate my time to other matters, I cannot do that now, so here we are.

If you like the idea of this music player lemme know, it will make me extremely happy :) and if you find a bug (which you surely will) or have some suggestions regarding how I could change some parts, do let me know

aha almost forgot, you can drag and drop your music files, add an entire folder or add songs using the provided buttons, they are easy to find and use

PS: Khi means boar in my language, the name is a sort of an inside joke and I decided to stick with it :)



![Khi Player - sample](https://github.com/rushakh/Khi-Player/assets/151368929/41def25e-3629-4082-8ee4-468ced243e5f)




![Khi Player - sample 2](https://github.com/rushakh/Khi-Player/assets/151368929/eea616a7-0077-4185-9e07-07904c1c74df)
