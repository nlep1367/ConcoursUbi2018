# Concours universitaire Ubisoft 2018

## Installation et configuration initiale de Git en local

1) Creer un dossier en local pour votre repertoire Git local
```
cd Document
mkdir Clone
cd Clone
```

2) Cloner le répertoire en local
```
git clone https://github.com/nlep1367/ConcoursUbi2018.git
```

3) Ajouter le répertoire qui sera le upstream, i.e. nlep1367/ConcoursUbi2018
```
git remote add upstream https://github.com/nlep1367/ConcoursUbi2018.git
```

4) Creer votre branche personnelle
```
git checkout -b naomi_personal
git push -u origin naomi_personal
```