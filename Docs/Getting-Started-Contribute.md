# Getting Started - Contribute

## Overview

* [Downloading The Repository](#downloading-the-repository)
* [Rules and Etiquette](#rules-and-etiquette)




## Downloading The Repository

The process of downloading the repository for contribution is only a little different from [Getting Started - Use](https://github.com/malware-dev/MDK-Mockups/blob/master/Docs/Getting-Started-Use.md). Rather than checking out the repository normally, you will need to follow [GitHub's instructions on how to Fork a repository](https://help.github.com/articles/fork-a-repo/). Other than that the process is identical.



Once you've made a change you wish to share, you will need to [create a pull request](https://help.github.com/articles/creating-a-pull-request-from-a-fork/). This will become a merge task that one of the team administrators can deal with and merge. Once this is done it is available for anyone who updates their repository.




## Rules and Etiquette

* _All_ `.cs` files **must** end with the suffix `.debug.cs`, not just `.cs`. This is so MDK can exclude these files when deploying a script.
* All mockup classes should be _public_. 
* Do not make your mockups work via nonstandard behavior. It should replicate game behavior or not at all.
* While there's **no requirement** to completely finish every single feature of a block, please make sure the parts you _do not_ include throws `NotImplementedException`. Obviously, the more you complete before creating your pull request, the better.
* It's considered courteous to follow the coding standards. There is a risk that your pull request might be rejected if it deviates too far from the standards.
* Everything is open for contribution, including this documentation. This is the reason it's stored directly in the repository rather than the wiki. The better documentation, the easier for people to get started, the more people we get contributing.
* Make sure your contributions do not break something. If it does, and you think your change is important, make contact with one of the project administrators - either via the Issues or via Keen's Discord, and argue your case.
* Make sure your contributions are flexible. Others may want to expand on it.
* Comment your contributions (documentation comments is fair enough) and make sure your code is readable.
* **This project relies on cooperation.** It will fall apart if we do not, so we will not tolerate bad behavior.
