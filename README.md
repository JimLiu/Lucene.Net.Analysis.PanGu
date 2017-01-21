Lucene.Net.Analysis.PanGu
=========================

盘古分词(http://pangusegment.codeplex.com/ )，由于老版本不支持最新Lucene.Net 4.8，对其进行了升级，可以支持最新的Lucene.Net 4.8 for .NET Core。可以直接NuGet安装。

另外把词库打包到dll文件里面了，无需拷贝词库，但是在本地约定的相对路径存放词库文件的话，仍然会优先加载本地文件系统的词库。

使用说明
=========================

* Lucene.Net： https://github.com/apache/lucene.net
* 盘古分词： http://pangusegment.codeplex.com/ 

主要依赖项的NuGet包暂时寄存在本项目Release下。

Special thanks to: 
* https://github.com/conniey/lucenenet, a kind man from Microsoft who made the .NET Core version of Luccen.Net.
* https://github.com/ouraspnet/OurAspNet.Lucene.Net.Analysis.PanGu, a kind man who made the .NET Core version of PanGu lib.