# EZInstall

I wanted a simple Windows installer for my programs so I can ship them as a single file while having the benefits of multi-file applications. With just a few lines of code you can get a simple background installer up and running.



EZInstall while compile to a DLL for portability, however it is recommended you compile it directly into your installer so then it's only 1 file. The other options is to use an assembly combiner tool to merge EZInstall with your installer project.



## Features

* Automatically execute program once installed
* Create a Start Menu shortcut
* Runs in background