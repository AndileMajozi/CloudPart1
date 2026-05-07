EaseEvents - CLDV6211 POE Part 1 and Part 2

How to run:
1. Open EventEase.csproj in Visual Studio 2022.
2. Make sure the .NET 8 SDK is installed.
3. Restore NuGet packages when Visual Studio asks.
4. Start Azurite before testing image upload:
   - Visual Studio: Tools > Command Line > Developer PowerShell, then run: azurite
   - Or open Azure Storage Explorer and start/connect to the local emulator.
5. Press F5 or click the HTTPS profile.
6. The app creates the SQL LocalDB database automatically using EnsureCreated().

Important screenshots for submission:
- Home page running on localhost.
- Venue CRUD screens.
- Event CRUD screens.
- Booking screen showing event and venue names.
- Double-booking validation message.
- Delete restriction message for venue/event linked to a booking.
- Azure Storage Explorer showing the venue-images container and uploaded files.
- Application displaying uploaded venue images.

Application name used in the UI: EaseEvents.
