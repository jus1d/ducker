# ducker
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
* **Don't forget about `config.json` file.** Set up prefix, token and other in `config.json` file, than move it to the same folder as your executable file

## Commands List

### Commands
| Command       | Description                                                               | Arguments              |
|---------------|---------------------------------------------------------------------------|------------------------|
| `help`        | Send help list to current channel                                         | `none / command`       |
| `random`      | Send  random value in your range from min to max value to current channel | `min value, max value` |
| `avatar`      | Send user's avatar and it's link to current channel                       | `user`                 |
| `invite-link` | Send invite link for this bot to current channel                          | `none`                 |

### Admin Commands
| Command               | Description                                                                                                                                                                            | Arguments                                      |
|-----------------------|----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|------------------------------------------------|
| `kick`                | Kick mentioned user from current server                                                                                                                                                | `member, reason`                               |
| `ban`                 | Ban mentioned user in current server                                                                                                                                                   | `user, reason`                                 |
| `mute`                | Mute mentioned member                                                                                                                                                                  | `member`                                       |
| `unmute`              | Unmute mentioned member                                                                                                                                                                | `member`                                       |
| `clear`               | Clear `amount` messages from current channel                                                                                                                                           | `amount`                                       |
| `add-role`            | Add a role to mentioned user                                                                                                                                                           | `member, role`                                 |
| `remove-role`         | Remove role from mentioned user                                                                                                                                                        | `member, role`                                 |
| `reaction-role-embed` | Send an embed with buttons, by press there you will granted a role                                                                                                                     | `none`                                         |
| `activity`            | Change bot activity                                                                                                                                                                    | `playing / streaming`                          |
| `embed`               | Create, and sends an embed  with your title, description, title URL, image (All optional, but title or description must be. If you use -del flag, message with config will be deleted) | `title, description, image, title URL, -del`   |
| `stream`              | Send stream announcement                                                                                                                                                               | ` none / stream description `                  |

### Music Commands
| Command       | Description                                                                       | Arguments      |
|---------------|-----------------------------------------------------------------------------------|----------------|
| `play`        | Start playing music from youtube, spotify or soundcloud by link or search request | `url / search` |
| `pause`       | Pause now playing music                                                           | `none`         |
| `stop`        | Permanently stop now playing music (can't use -play command to resume playing)    | `none`         |
| `resume`      | Resume playing music                                                              | `none`         |
| `skip`        | Skip current playing track to next in queue                                       | `none`         |
| `repeat`      | Repeat current track                                                              | `none`         |
| `queue`       | Send queue list to current channel                                                | `none`         |
| `clear-queue` | Clear queue list                                                                  | `none`         |
| `phonk`       | Starts playing Memphis Phonk                                                      | `none`         |

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
