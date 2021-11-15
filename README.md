# ducker Bot
## INFO
> This bot is under development, it is intended for use in particular by server administrators.

Standart bot prefix is `-`, but you can also use the `/` commands by typing `/` in your input discord line.

## Commands
| Command | Description | Usage |
|---|----| --- |
|`help`| Bot will send help list to current channel | `-help / <command>` |
|`avatar`| Bot will send embed with users avatar to current channel | `-avatar <user>` |
|`invitelink`| Bot will send invite link for this bot to current channel | `-invitelink` |
|`random`| Bot will send  random value in your range from min to max value to current channel | `-random <min> <max>` |
|`play`| Starts playing music from youtube by link or search request | `-play <url> /` `<search> / (resume)` |
|`pause`| Pause now playing music (can use -play command to resume playing) | `-pause` |
|`stop`| Permanently stop now playing music (can't use -play command to resume playing) | `-stop` |
|`ban`| Ban mentioned user in current server * | `-ban <user>` |
|`kick`| Kick mentioned user from current server * | `-kick <user>` |
|`clear`| Clear certain number of messages in current channel * | `-clear <amount>` |
|`embed`| Send embed to current channel with your title, description, title URL, image (all optional, but title or description must be, if you use -del flag, message with config will be deleted) * | `-embed -t <title> -d` `<description> ...` |
|`poll`| Creates embed with poll with your description in current channel, and create on this message :white_check_mark: and :x: * | `-poll <poll` `description>` |
> (* - admin commands)
