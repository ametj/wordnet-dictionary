# WordNet Dictionary

Simple dictionary application using [WordNet](https://wordnet.princeton.edu/) for word definitions.


# Installation

1. Copy WordNet data file to `Data` folder, for example from [English Wordnet](https://github.com/globalwordnet/english-wordnet). Currently only LMF (xml) format is supported.

2. Run `WordNet.Import`, which will generate Sqlite database `WordNet.db` from the specified WordNet file in `Data` folder.

3. `WordNet.db` is automatically coppied from `Data` folder to output on build.

4. Done. You can now run `WordNet.Wpf` Dictionary.
