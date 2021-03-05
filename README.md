# SortZam
Sort, Recognize and Edit your musics file ID3 tags.

## Introduction
This application is made by DJs for DJs and tidy music enthusiast.
It allows you to sort automatically (or not), your musics tags files :
-   Specify a directory for searching music files
-   Analyze the fingerprint of your file to find automatically the title, the artist(s), the album, the year, and the gender of your music.
-   Select the best result proposition (or do it automatically)
-   Check the values music per music, and change it if you like
-   Save all the tags files in one action

## How It Works
This a C# .net-core application in stand alone mode, to be OS agnostic.
Music Recognition is provided by ACRCloud API. ACRCloud is the most advanced Acoustic Fingerprint Algorithm. It has the world's largest database of music files (over 72 million tracks which is constantly being updated), and was rated the No.1 platform for Audio Fingerprinting at the Music Information Retrieval Evaluation eXchange (MIREX). ACRCloud Music Recognition Services allow developers to match directly with online music services (Spotify, Deezer, Youtube etc) and standard codes such as ISRC and UPC. 
More info on their [website].

## Prerequisite
To use this application, you need (one or more) ACRCloud account, with valid API tokens :
-   If you don't have an account yet, let's go to [Sign UP] and submit your inscription
-   Then, visit your [Console administration interface] and log in.
-   Find the `Audio & Video Recognition` product and subscribe
-   Click on `Create Project` button, select "Line-in Audio", and choose your 3rd Party source for music Recognition (Spotify, deezer, youtube, isrc, upc or all of them).
-   Get the Host, Access Key and Secret Key values to specify them when starting the application SortZam.

## Pricing
The Application is free and open-source with no license limit. But she is based on the ACRCloud API where the price are described here :
| Valid Requests (per month)        | Price (per 1000 Valid Requests)      |
| ------|-----|-----|
| First 5000  	| Free 	|
| Next 20000  	| 3$ 	|
| Next 25000  	| 2.5$ 	|
| Next 55000  	| 2.2$ 	|
| Over 100 000  	| 1.7$ 	|
(prices may change, non-contractual data)

[website]:    https://www.acrcloud.com/
[Sign UP]:    https://console.acrcloud.com/signup#/register
[Console administration interface]:    https://console.acrcloud.com/signin/home