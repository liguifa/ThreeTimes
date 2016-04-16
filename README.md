# 三度调查系统  一个提供员工调查管理功能的网站

## 功能介绍

三度调查系统的实施目的是对被调查公司以及下属子公司的员工进行工作满意度、忠诚度、敬业度调查，完成时导出调查问卷数据到Excel。
软件整体应为B/S模式，系统应能够同时对多个乙方进行调查，并且乙方及其员工信息由乙方总公司（一个乙方可以包含多个子公司，并且对子公司有明确的分级）HR交与甲方联系人，甲方接收到信息后，将信息导入系统，设置问卷后，开始对乙方员工发送邮件进行调查，乙方员工接收到Email后，得到登陆地址以及用户名和密码，在规定时间内登陆后确认信息，填写问卷。待乙方员工或者时间截止或者甲方强行截止调查后，乙方员工不可再登陆，甲方导出调查数据，使用其他数据统计软件对数据处理后，交予乙方则调查完成。
本系统只负责员工调查，不负责数据处理。公司联系人（HR）只可以查看公司结构，和调查进度。员工登陆后，首先确认信息，然后填写问卷，全部选择一个答案或者未完成答题，可视为无效问卷，不允许提交。

<br />1、系统管理:1)系统管理 2)角色管理 3)用户管理 4)菜单管理 5)权限管理
<br />2、公司信息管理：1)添加集团公司 2)子公司管理 3)调查人员信息管理 4)题库管理 5)调查管理。
<br />3、问卷管理：1)建立问卷 2)时间设置 3)问卷问题管理 4)问卷预览 5)问卷修改 6)调查时间设置
<br />4、答卷进度监控
<br />5、导出问卷
<br />6、问卷分析管理

## 项目实现

使用C#编写，使用.Net MVC+EF+三层架构实现,前端使用Esyui实现.

## 使用例图

![image](https://github.com/liguifa/threetimes/blob/master/1.png)
![image](https://github.com/liguifa/threetimes/blob/master/2.png)
![image](https://github.com/liguifa/threetimes/blob/master/3.png)
![image](https://github.com/liguifa/threetimes/blob/master/4.png)
![image](https://github.com/liguifa/threetimes/blob/master/5.png)

## 问题反馈

有问题的话，<a href="https://github.com/liguifa/threetimes/issues/new">报个issue</a>吧！

## 关于作者

本项目的作者是C#后端工程师；  
他的主页： http://liguifa.wang  
他的知乎： https://www.zhihu.com/people/guifa-li  
他的邮箱： http://github.liguifa@outlook.com 
<br />欢迎造访交流！
