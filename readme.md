UO-MES
======

UO-Universal Open MES于2009年2月7日开始开发，Active开发时间一年，现在处于废弃状态，新一代也是最终版的MES[参见这里](https://github.com/Eric-Guo/cloud-mes)

人员列表：
Diego
Eric
Jesse

注意：如何架设开发环境请参考《UO MES开发者手册》的建构开发环境一节

目录结构由Eric控制，子目录说明如下：
* Documents		项目文档
* Documents\Technical	技术文档
* Documents\Technical\Getting Started with UO-MES 开始介绍
* Documents\Technical\Developer Guide 开发者文档
* Documents\Technical\UO Services Reference 服务参考
* SRC			项目源代码
* SRC\DB_Batch		数据库维护脚本
* SRC\MES_Model		可直接导入MES的Excel建模文件
* SRC\ReferenceDLL	第三方相关引用DLL
* SRC\UO_MES		项目核心主要代码包括：
* SRC\UO_MES\UO_Model		可存入数据库的工厂模型定义
* SRC\UO_MES\UO_Service		对工厂模型进行操作的事务服务
* SRC\UO_MES\UO_ExcelModeler	同Excel集成的工厂模型导入工具
* SRC\UO_MES\UO_UnitTest		UO-MES系统的功能单元测试
* SRC\UO_MES\UO_WebApplication	UO-MES的网页应用程序（仅界面）
* Web			公司静态网页

MES Excel Model修改添加流程：
MES Excel Model是实际运行的工厂模型，利用Excel作为建模工具方便初始数据处理和导入。
1. 复制S10 Physical Basic Model.xls中的已有Sheet，改名为待添加Model类的类名
2. 按照Model类定义添加相应数据字段
3. 添加数据，其中Excel的一行代表一个Model类的实例
4. 其中Execute一列中输入命令，支持INSERT, UPDATE, REMOVE三个命令
5. 保存Excel后，点击Excel帮助右边的新增菜单，选择“上传UO-MES模型”命令修改维护UO-MES模型（若找不到此菜单，确认ExcelModeler项目成功建构并注册）

注意：递交到SVN的模型文件，应该处于INSERT状态，且经过测试。

WebApplication支持通过CSS（推荐）和MasterPage换肤，CSS Zen Garden对应的UO MES CSS样式表名称如下:
1. intro->header
2. pageHeader->pageheader
3. quickSummary->mainnav-container
4. preamble->breadcrumb-container
5. supportingText->content-container
6. linkList->content-side