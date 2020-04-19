# DNC-Authentication


## ROLES VS CLAIMS VS POLICY

### ROLES
![img](https://imagizer.imageshack.com/img924/1177/Mo6RmT.png)
- A user is either in it or not
  - kind of like booleans
- One user can have many roles
- Encapsulating
  - user
  - functions
- You can use roles to enforce policies

**FOLDER: Roles -> Controllers -> RolesController**
### Claims

![img 1](https://imagizer.imageshack.com/img924/7936/e2w9FQ.png)
![img 2](https://imagizer.imageshack.com/img923/5056/qMntBB.png)
- Is a key value pair (img 1)
- Describes a user
	- Is a user property
- A claim belongs to a user
  - A user can have many claims (img 2)

### Policies
![img 3](https://imagizer.imageshack.com/img922/2243/3beoNB.png)
- Is an authorization function
- You can use roles to enforce policies

**FOLDER: Roles -> Controllers -> PoliciesController**


##### Sources
[rawcoding](https://www.youtube.com/watch?v=cbtK3U2aOlg)
[rawcoding github](https://www.youtube.com/redirect?q=https%3A%2F%2Fgithub.com%2FT0shik%2Faspnetcore3-authentication&redir_token=rkrQ8qoh4X3vRg3rm_v0fh4UYQ58MTU4NzM5NjY0MEAxNTg3MzEwMjQw&event=video_description&v=IjbtWPXVJGw)
