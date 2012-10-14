#region Copyright and License
/* Copyright (c) 2010 Stephen Styrchak
 * 
 * Microsoft Public License (Ms-PL)
 * 
 * This license governs use of the accompanying software. If you use the
 * software, you accept this license. If you do not accept the license, do not
 * use the software.
 * 
 * 1. Definitions
 * 
 * The terms "reproduce," "reproduction," "derivative works," and "distribution"
 * have the same meaning here as under U.S. copyright law.
 * 
 * A "contribution" is the original software, or any additions or changes to the
 * software.
 * 
 * A "contributor" is any person that distributes its contribution under this license.
 * 
 * "Licensed patents" are a contributor's patent claims that read directly on its
 * contribution.
 * 
 * 2. Grant of Rights
 * 
 * (A) Copyright Grant- Subject to the terms of this license, including the license
 *     conditions and limitations in section 3, each contributor grants you a non-
 *     exclusive, worldwide, royalty-free copyright license to reproduce its contribution,
 *     prepare derivative works of its contribution, and distribute its contribution or
 *     any derivative works that you create.
 * 
 * (B) Patent Grant- Subject to the terms of this license, including the license
 *     conditions and limitations in section 3, each contributor grants you a non-exclusive,
 *     worldwide, royalty-free license under its licensed patents to make, have made, use,
 *     sell, offer for sale, import, and/or otherwise dispose of its contribution in the
 *     software or derivative works of the contribution in the software.
 * 
 * 3. Conditions and Limitations
 * 
 * (A) No Trademark License- This license does not grant you rights to use any contributors'
 *     name, logo, or trademarks.
 * 
 * (B) If you bring a patent claim against any contributor over patents that you claim are
 *     infringed by the software, your patent license from such contributor to the software
 *     ends automatically.
 * 
 * (C) If you distribute any portion of the software, you must retain all copyright, patent,
 *     trademark, and attribution notices that are present in the software.
 * 
 * (D) If you distribute any portion of the software in source code form, you may do so only
 *     under this license by including a complete copy of this license with your distribution.
 *     If you distribute any portion of the software in compiled or object code form, you may
 *     only do so under a license that complies with this license.
 * 
 * (E) The software is licensed "as-is." You bear the risk of using it. The contributors give
 *     no express warranties, guarantees or conditions. You may have additional consumer
 *     rights under your local laws which this license cannot change. To the extent permitted
 *     under your local laws, the contributors exclude the implied warranties of
 *     merchantability, fitness for a particular purpose and non-infringement. 
 */
#endregion
#region Using Statements
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace DebugPipeline
{
    class Program
    {
        private const string ProjectToDebug = @"C:\Users\diego\Documents\Visual Studio 2010\Projects\ProjetoFinal\ProjetoFinal\ProjetoFinalContent\ProjetoFinalContent.contentproj";

        private const string SingleItem = @"levels\complex.oel";

        private const GraphicsProfile XnaProfile = GraphicsProfile.Reach;

        private const TargetPlatform XnaPlatform = TargetPlatform.Windows;

        private const LoggerVerbosity LoggingVerbosity = LoggerVerbosity.Normal;

        #region MSBuild hosting and execution

        [STAThread]
        static void Main()
        {
            if (!File.Exists(ProjectToDebug))
            {
                throw new FileNotFoundException(String.Format("The project file '{0}' does not exist. Set the ProjectToDebug field to the full path of the project you want to debug.", ProjectToDebug), ProjectToDebug);
            }
            if (!String.IsNullOrEmpty(SingleItem) && !File.Exists(Path.Combine(WorkingDirectory, SingleItem)))
            {
                throw new FileNotFoundException(String.Format("The project item '{0}' does not exist. Set the SingleItem field to the relative path of the content item you want to debug, or leave it empty to debug the whole project.", SingleItem), SingleItem);
            }
            Environment.CurrentDirectory = WorkingDirectory;

            var globalProperties = new Dictionary<string, string>();

            globalProperties.Add("Configuration", Configuration);
            globalProperties.Add("XnaProfile", XnaProfile.ToString());
            globalProperties.Add("XNAContentPipelineTargetPlatform", XnaContentPipelineTargetPlatform);
            globalProperties.Add("SingleItem", SingleItem);
            globalProperties.Add("CustomAfterMicrosoftCommonTargets", DebuggingTargets);

            var project = ProjectCollection.GlobalProjectCollection.LoadProject(ProjectName, globalProperties, MSBuildVersion);
            bool succeeded = project.Build("rebuild", Loggers);

            
            Debug.WriteLine("Build " + (succeeded ? "Succeeded." : "Failed."));
        }

        #region Additional, rarely-changing property values

        private const string Configuration = "Debug";
        private const string MSBuildVersion = "4.0";

        private static IEnumerable<ILogger> Loggers
        {
            get
            {
                return new ILogger[] { new ConsoleLogger(LoggingVerbosity) };
            }
        }

        private static string WorkingDirectory
        {
            get { return Path.GetDirectoryName(Path.GetFullPath(ProjectToDebug)); }
        }

        private static string BuildToolDirectory
        {
            get
            {
                string startupExe = System.Reflection.Assembly.GetEntryAssembly().Location;
                return Path.GetDirectoryName(startupExe);
            }
        }

        private static string ProjectName
        {
            get { return Path.GetFileName(Path.GetFullPath(ProjectToDebug)); }
        }

        private static string XnaContentPipelineTargetPlatform
        {
            get
            {
                return XnaPlatform.ToString();
            }
        }

        public static string DebuggingTargets
        {
            get
            {
                if (String.IsNullOrEmpty(SingleItem))
                {
                    return String.Empty;
                }

                string targetsPath = @"Targets\Debugging.targets";
                return Path.Combine(BuildToolDirectory, targetsPath);
            }
        }

        #endregion

        #endregion
    }
}
