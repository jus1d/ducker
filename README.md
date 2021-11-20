
# ducker Discord Bot
The bot is developing in C# by jus1d, based on DSharpPlus library. This bot currently still under developing, most changes is coming. 
####
Standard bot prefix is `-`, but you can also use the slash commands by typing `/` in your input discord line.

### Developer Mode
To use some commands you'll need member's, channel's or guild's IDs. To use IDs, you have to turn on the discord developer mode. **Go to: User settings -> Advanced -> Developer Mode**

## To Install
* Clone this repo to your computer
* Compile this project with Visual Studio or Rider, and it will be ready
* **Don't forget about `config.json` file.** Set up prefix and token in `config.json` file, than move it to the same folder as your executable

## Commands List:

### Commands
| Command | Description | Arguments |
|---|----|---|
| `help` | Bot will send help list to current channel | `none / command` |
| `random` | Bot will send  random value in your range from min to max value to current channel | `min value, max value` |
| `avatar` | Bot will send embed with users avatar to current channel | `user` |
| `invitelink` | Bot will send invite link for this bot to current channel | `none` |

### Admin Commands
| Command | Description | Arguments |
|---|----|---|
| `kick` | Kicks mentioned user from current server | `member, reason` |
| `ban` | Bans mentioned user in current server | `user, reason` |
| `clear` | Clear `amount` messages from current channel | `amount` |
| `embed` | Creates, and sends an embed  with your title, description, title URL, image (All optional, but title or description must be. If you use -del flag, message with config will be deleted) | `title, description, image, title URL, -del flag` |
| `poll` | Creates embed with poll with your description in current channel, and create on this message :white_check_mark: and :x: | `description` |
| `addrole` | Adds a role to mentioned user | `member, role` |
| `removerole` | Removes role from mentioned user | `member, role` |

### Music Commands
| Command | Description | Arguments |
|---|----|---|
| `play` | Starts playing music from youtube by link or search request | `url / search` |
| `pause` | Pause now playing music | `none` |
| `stop` | Permanently stop now playing music (can't use -play command to resume playing) | `none` |
| `resume` | Resume playing music | `none` |
