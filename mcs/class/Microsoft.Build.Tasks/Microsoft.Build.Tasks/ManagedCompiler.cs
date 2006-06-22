//
// ManagedCompiler.cs: Task for compilers
//
// Author:
//   Marek Sieradzki (marek.sieradzki@gmail.com)
// 
// (C) 2005 Marek Sieradzki
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#if NET_2_0

using System;
using System.Collections;
using System.IO;
using System.Text;
using Microsoft.Build.Framework;

namespace Microsoft.Build.Tasks {
	public abstract class ManagedCompiler : ToolTaskExtension {
	
		protected ManagedCompiler ()
		{
		}

		[MonoTODO]
		protected internal override void AddCommandLineCommands (
						 CommandLineBuilderExtension commandLine)
		{
		}

		[MonoTODO]
		protected internal override void AddResponseFileCommands (
						 CommandLineBuilderExtension commandLine)
		{
			commandLine.AppendSwitchIfNotNull ("/lib:", AdditionalLibPaths, ",");
			commandLine.AppendSwitchIfNotNull ("/addmodule:", AddModules, ",");
			//commandLine.AppendSwitchIfNotNull ("/codepage:", CodePage.ToString ());
			//debugType
			commandLine.AppendSwitchIfNotNull ("/define:", DefineConstants);
			//delaySign
			if (EmitDebugInformation)
				commandLine.AppendSwitch ("/debug");
			//fileAlignment
			commandLine.AppendSwitchIfNotNull ("/keycontainer:", KeyContainer);
			commandLine.AppendSwitchIfNotNull ("/keyfile:", KeyFile);
			// FIXME: add ids from metadata
			if (LinkResources != null) {
				foreach (ITaskItem item in LinkResources) {
						commandLine.AppendSwitchIfNotNull ("/linkresource:", item.ItemSpec);
				}
			}
			commandLine.AppendSwitchIfNotNull ("/main:", MainEntryPoint);
			if (NoConfig)
				commandLine.AppendSwitch ("/noconfig");
			if (Optimize)
				commandLine.AppendSwitch ("/optimize");
			if (OutputAssembly != null)
				commandLine.AppendSwitchIfNotNull ("/out:", OutputAssembly.ItemSpec);
			if (References != null) {
				foreach (ITaskItem item in References) {
					commandLine.AppendSwitchIfNotNull ("/reference:", item.ItemSpec);
				}
			}
			if (Resources != null) {
				foreach (ITaskItem item in Resources) {
						commandLine.AppendSwitchIfNotNull ("/resource:", item.ItemSpec);
				}
			}
			if (ResponseFiles != null) {
				foreach (ITaskItem item in ResponseFiles) {
						commandLine.AppendFileNameIfNotNull (String.Format ("@{0}",item.ItemSpec));
				}
			}
			if (Sources != null) {
				foreach (ITaskItem item in Sources) {
						commandLine.AppendFileNameIfNotNull (item.ItemSpec);
				}
			}
			commandLine.AppendSwitchIfNotNull ("/target:", TargetType);
			if (TreatWarningsAsErrors)
				commandLine.AppendSwitch ("/warnaserror");
			commandLine.AppendSwitchIfNotNull ("/win32icon:", Win32Icon);
			commandLine.AppendSwitchIfNotNull ("/win32res:", Win32Resource);
		}

		[MonoTODO]
		protected bool CheckAllReferencesExistOnDisk ()
		{
			foreach (ITaskItem item in (ITaskItem[])Bag ["References"]) 
				if (File.Exists (item.GetMetadata ("FullPath")) == false)
					return false;
			return true;
		}

		[MonoTODO]
		protected void CheckHostObjectSupport (string parameterName,
						       bool resultFromHostObjectSetOperation)
		{
		}
		
		[MonoTODO]
		protected override bool HandleTaskExecutionErrors ()
		{
			return true;
		}
		
		[MonoTODO]
		protected bool ListHasNoDuplicateItems (ITaskItem[] itemList,
							string parameterName)
		{
			Hashtable items = new Hashtable ();
			
			foreach (ITaskItem item in itemList) {
				if (items.Contains (item.ItemSpec))
					items.Add (item.ItemSpec, null);
				else
					return false;
			}
			
			return true;
		}

		[MonoTODO]
		protected override bool ValidateParameters ()
		{
			return true;
		}

		public string[] AdditionalLibPaths {
			get { return (string[]) Bag ["AdditionalLibPaths"]; }
			set { Bag ["AdditionalLibPaths"] = value; }
		}

		public string[] AddModules {
			get { return (string[]) Bag ["AddModules"]; }
			set { Bag ["AddModules"] = value; }
		}

		public int CodePage {
			get { return GetIntParameterWithDefault ("CodePage", 0); }
			set { Bag ["CodePage"] = value; }
		}

		public string DebugType {
			get { return (string) Bag ["DebugType"]; }
			set { Bag ["DebugType"] = value; }
		}

		public string DefineConstants {
			get { return (string) Bag ["DefineConstants"]; }
			set { Bag ["DefineConstants"] = value; }
		}

		public bool DelaySign {
			get { return GetBoolParameterWithDefault ("DelaySign", false); }
			set { Bag ["DelaySign"] = value; }
		}

		public bool EmitDebugInformation {
			get { return GetBoolParameterWithDefault ("EmitDebugInformation", false); }
			set { Bag ["EmitDebugInformation"] = value; }
		}

		public int FileAlignment {
			get { return GetIntParameterWithDefault ("FileAlignment", 0); }
			set { Bag ["FileAlignment"] = value; }
		}

		protected bool HostCompilerSupportsAllParameters {
			get { return true; }
			set { Bag ["HostCompilerSupportsAllParameters"] = value; }
		}

		public string KeyContainer {
			get { return (string) Bag ["KeyContainer"]; }
			set { Bag ["KeyContainer"] = value; }
		}

		public string KeyFile {
			get { return (string) Bag ["KeyFile"]; }
			set { Bag ["KeyFile"] = value; }
		}

		public ITaskItem[] LinkResources {
			get { return (ITaskItem[]) Bag ["LinkResources"]; }
			set { Bag ["LinkResources"] = value; }
		}

		public string MainEntryPoint {
			get { return (string) Bag ["MainEntryPoint"]; }
			set { Bag ["MainEntryPoint"] = value; }
		}

		public bool NoConfig {
			get { return GetBoolParameterWithDefault ("NoConfig", false); }
			set { Bag ["NoConfig"] = value; }
		}

		public bool NoLogo {
			get { return GetBoolParameterWithDefault ("NoLogo", false); }
			set { Bag ["NoLogo"] = value; }
		}

		public bool Optimize {
			get { return GetBoolParameterWithDefault ("Optimize", false); }
			set { Bag ["Optimize"] = value; }
		}

		[Output]
		public ITaskItem OutputAssembly {
			get { return (ITaskItem) Bag ["OutputAssembly"]; }
			set { Bag ["OutputAssembly"] = value; }
		}

		public ITaskItem[] References {
			get { return (ITaskItem[]) Bag ["References"]; }
			set { Bag ["References"] = value; }
		}

		public ITaskItem[] Resources {
			get { return (ITaskItem[]) Bag ["Resources"]; }
			set { Bag ["Resources"] = value; }
		}

		public ITaskItem[] ResponseFiles {
			get { return (ITaskItem[]) Bag ["ResponseFiles"]; }
			set { Bag ["ResponseFiles"] = value; }
		}

		public ITaskItem[] Sources {
			get { return (ITaskItem[]) Bag ["Sources"]; }
			set { Bag ["Sources"] = value; }
		}

		protected override Encoding StandardOutputEncoding {
			get { return Console.Error.Encoding; }
		}

		// FIXME: hack to get build of hello world working
		public string TargetType {
			get { return ((string) Bag ["TargetType"]).ToLower (); }
			set { Bag ["TargetType"] = value; }
		}

		public bool TreatWarningsAsErrors {
			get { return GetBoolParameterWithDefault ("TreatWarningsAsErrors", false); }
			set { Bag ["TreatWarningsAsErrors"] = value; }
		}

		public bool Utf8Output {
			get { return GetBoolParameterWithDefault ("Utf8Output", false); }
			set { Bag ["Utf8Output"] = value; }
		}

		public string Win32Icon {
			get { return (string) Bag ["Win32Icon"]; }
			set { Bag ["Win32Icon"] = value; }
		}

		public string Win32Resource {
			get { return (string) Bag ["Win32Resource"]; }
			set { Bag ["Win32Resource"] = value; }
		}
	}
}

#endif
