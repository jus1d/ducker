
# ducker Discord Bot
The bot is developing in C# by jus1d, based on DSharpPlus library. This bot currently still under developing, most changes is coming. 
####
Standard bot prefix is `-`, but you can also use the slash commands by typing `/` in your input discord line.

### Developer Mode
To use some commands you'll need member's, channel's or guild's IDs. To use IDs, you have to turn on the discord developer mode. **Go to: User settings -> Advanced -> Developer Mode**

## To Install
* Clone this repo to your computer:
  ```bash 
  git clone https://github.com/jus1d/duckerBot.git
* Compile this project with Visual Studio or Rider, and it will be ready
* **Don't forget about `config.json` file.** Set up prefix and token in `config.json` file, than move it to the same folder as your executable

## Commands List:

### Commands
| Command | Description | Arguments |
|---|----|---|
| `help` | Bot will send help list to current channel | `none / command` |
| `random` | Bot will send  random value in your range from min to max value to current channel | `min value, max value` |
| `avatar` | Bot will send embed with users avatar to current channel | `user` |
| `invite-link` | Bot will send invite link for this bot to current channel | `none` |

### Admin Commands
| Command | Description | Arguments |
|---|----|---|
| `kick` | Kicks mentioned user from current server | `member, reason` |
| `ban` | Bans mentioned user in current server | `user, reason` |
| `clear` | Clear `amount` messages from current channel | `amount` |
| `add-role` | Adds a role to mentioned user | `member, role` |
| `remove-role` | Removes role from mentioned user | `member, role` |
| `mute` | Mutes mentioned member | `member` |
| `unmute` | Unmutes mentioned member | `member` |
| `rrembed` | Sends an embed with buttons, by press there you will granted a role | `none` |
| `activity` | Changes bot activity | `playing / streaming` |
| `embed` | Creates, and sends an embed  with your title, description, title URL, image (All optional, but title or description must be. If you use -del flag, message with config will be deleted) | `title, description, image, title URL, -del flag` |

### Music Commands
| Command | Description | Arguments |
|---|----|---|
| `play` | Starts playing music from youtube, spotify or soundcloud by link or search request | `url / search` |
| `pause` | Pause now playing music | `none` |
| `stop` | Permanently stop now playing music (can't use -play command to resume playing) | `none` |
| `resume` | Resume playing music | `none` |
| `skip` | Skips current playing track to next in queue | `none` |
| `queue` | Sends queue list to current channel | `none` |
| `clear-queue` | Clear queue list | `none` |
| `phonk` | Starts playing Memphis Phonk | `none` |

## Contributing
Contributions are what make the open source community such an amazing place to be learn, inspire, and create. Any contributions you make are greatly appreciated.

1. Fork the Project
2. Create your feature branch
3. Commit your changes
4. Push to the branch
5. Open a Pull Request

## License

Distributed under the MIT License. See `LICENSE` for more information.

## Contact

[@jus1d](https://twitter.com/jus1dq) - jus1dhah@gmail.com
