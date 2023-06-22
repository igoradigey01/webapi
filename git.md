## VS code git
[vscode&github](https://vscode.github.com)
## Git help about-remote-repositories
[doc git](https://docs.github.com/ru/get-started/getting-started-with-git/about-remote-repositories)
## create
 1. create in github
 2. init in project
- git init
- git add README.md
- git commit -m "first commit"
- git branch -M main
- git remote add origin https://github.com/igoradigey01/webapi.git
- git push -u origin main

 ## Commit names
 ```
 Type: chore, docs, feat, fix, refactor, style, or test.

 chore: add Oyster build script
docs: explain hat wobble
feat: add beta sequence
fix: remove broken confirmation message
refactor: share logic between 4d3d3d3 and flarhgunnstow
style: convert tabs to spaces
test: ensure Tayne retains clothing
 ```


## Git шпоргалка
  ```
  git init [project-name] Ч создать новый локальный репозиторий с заданным именем.
  git clone [url] Ч загрузить проект и его полную историю изменений.
  git branch -m <new name of branch> -переименовать текущюю ветку(main)
  git push origin <name of branch> -при несуществующей ветки git создаст ее автоматически.
  ### 
  git add . Ч сделать все измененные файлы готовыми дл€ коммита.
  git commit -m "[descriptive message]" Ч записать изменени€ с заданным сообщением.
  git commit --amend Ч добавить изменени€ к последнему коммиту.
  ###
  git branch Ч список всех локальных веток в текущей директории.
  git branch [branch-name] Ч создать новую ветку.
  git checkout [branch-name] Ч переключитьс€ на указанную ветку и обновить рабочую директорию.
  git checkout -b <name>  Ч переключитьс€ на созданную ветку.
  ###
  git merge [branch] Ч соединить изменени€ в текущей ветке с изменени€ми из заданной.
  git fetch   Ч загрузить с заданного удаленного репозитори€.
  git push Ч запушить текущую ветку в удаленную ветку.

  https://github.com/igoradigey01/wsv2
  
  ```