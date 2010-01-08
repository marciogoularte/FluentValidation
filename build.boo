project_name = "FluentValidation"
solution_file = "FluentValidation.sln"
solution_file_silverlight = "FluentValidation.Silverlight.sln"
project_configuration = "release"
build_dir = "build"
test_assemblies = "src/FluentValidation.Tests/bin/${project_configuration}/FluentValidation.Tests.dll"
build_silverlight = false
ncover_path = "C:/Program Files (x86)/ncover"

target default, (init, compile, test, deploy):
  pass

target ci, (init, silverlight, compile, coverage, package):
  pass
  
target init:
  rm(build_dir)
  rm("teamcity-info.xml")
  
desc "Enables silverlight build"
target silverlight:
  print "Silverlight build enabled"
  build_silverlight = true

desc "Compiles solution"
target compile:
  msbuild(file: solution_file, configuration: project_configuration)
  
  if build_silverlight:
    msbuild(file: solution_file_silverlight, configuration: project_configuration)

desc "Runs tests"
target test:
  nunit(assembly: test_assemblies)

desc "Creates release package"
target deploy:  
  #main binaries
  with FileList("src/FluentValidation/bin/${project_configuration}"):
    .Include("**/*")
    .ForEach def(file):
      file.CopyToDirectory("${build_dir}/${project_configuration}/FluentValidation")
  
  #Mvc integration
  with FileList("src/FluentValidation.Mvc/bin/${project_configuration}"):
    .Include("FluentValidation.Mvc.*")
    .ForEach def(file):
      file.CopyToDirectory("${build_dir}/${project_configuration}/MVC")
      
  #CSL integration
  with FileList("src/FluentValidation.CommonServiceLocator/bin/${project_configuration}"):
    .Include("FluentValidation.CommonServiceLocator.*")
    .Include("Microsoft.Practices.ServiceLocation.dll")
    .ForEach def(file):
      file.CopyToDirectory("${build_dir}/${project_configuration}/CommonServiceLocator")
  
  #xVal
  with FileList("src/FluentValidation.xValIntegration/bin/${project_configuration}"):
    .Include("FluentValidation.xValIntegration.*")
    .Include("xVal.dll")
    .ForEach def(file):
      file.CopyToDirectory("${build_dir}/${project_configuration}/xVal")
  
  #silverlight
  if build_silverlight:
    with FileList("src/FluentValidation.Silverlight/bin/${project_configuration}"):
      .Include("*.{dll,pdb,xml}")
      .ForEach def(file):
        file.CopyToDirectory("${build_dir}/${project_configuration}/Silverlight")
      
  #License/Changelog
  with FileList():
    .Include("License.txt")
    .Include("Changelog.txt")
    .ForEach def(file):
      file.CopyToDirectory("${build_dir}/${project_configuration}")
          
target package, (deploy):
  zip("${build_dir}/${project_configuration}", "${build_dir}/FluentValidation.zip")
  
target coverage:
  ncover_path = "c:/program files (x86)/ncover"
  app_assemblies = ("FluentValidation", "FluentValidation.Mvc", "FluentValidation.xValIntegration", "FluentValidation.CommonServiceLocator")
  teamcity_launcher = env("teamcity.dotnet.nunitlauncher")

  with ncover():
    .toolPath = "${ncover_path}/NCover.console.exe"
    .reportDirectory = "${build_dir}/Coverage"
    .workingDirectory = "src/FluentValidation.Tests/bin/${project_configuration}"
    .applicationAssemblies = app_assemblies
    .program = "${teamcity_launcher} v2.0 x86 NUnit-2.4.6"
    .testAssembly = "FluentValidation.Tests.dll"
    .excludeAttributes = "System.Runtime.CompilerServices.CompilerGeneratedAttribute"

  with ncover_explorer():
    .toolPath = "${ncover_path}/NCoverExplorer.console.exe"
    .project = "FluentValidation"
    .reportDirectory = "build/coverage"