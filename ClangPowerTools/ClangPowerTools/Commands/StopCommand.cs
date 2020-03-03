﻿using ClangPowerTools.Services;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using System;
using System.ComponentModel.Design;
using System.Windows.Forms;
using Task = System.Threading.Tasks.Task;

namespace ClangPowerTools.Commands
{
  /// <summary>
  /// Command handler
  /// </summary>
  public sealed class StopCommand : ClangCommand
  {
    #region Members

    private PCHCleaner mPCHCleaner = new PCHCleaner();

    #endregion


    #region Properties


    /// <summary>
    /// Gets the instance of the command.
    /// </summary>
    public static StopCommand Instance
    {
      get;
      private set;
    }


    #endregion


    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="StopCommand"/> class.
    /// Adds our command handlers for menu (commands must exist in the command table file)
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    private StopCommand(OleMenuCommandService aCommandService, CommandController aCommandController,
      AsyncPackage aPackage, Guid aGuid, int aId)
      : base(aPackage, aGuid, aId)
    {
      if (null != aCommandService)
      {
        var menuCommandID = new CommandID(CommandSet, Id);
        var menuCommand = new OleMenuCommand(aCommandController.Execute, menuCommandID);
        menuCommand.BeforeQueryStatus += aCommandController.OnBeforeClangCommand;
        menuCommand.Enabled = true;
        aCommandService.AddCommand(menuCommand);
      }
    }

    #endregion


    #region Public Methods


    /// <summary>
    /// Initializes the singleton instance of the command.
    /// </summary>
    /// <param name="package">Owner package, not null.</param>
    public static async Task InitializeAsync(CommandController aCommandController,
      AsyncPackage aPackage, Guid aGuid, int aId)
    {
      // Switch to the main thread - the call to AddCommand in StopClang's constructor requires
      // the UI thread.
      await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(aPackage.DisposalToken);

      OleMenuCommandService commandService = await aPackage.GetServiceAsync((typeof(IMenuCommandService))) as OleMenuCommandService;
      Instance = new StopCommand(commandService, aCommandController, aPackage, aGuid, aId);
    }


    public Task RunStopClangCommandAsync(bool background)
    {
      return Task.Run(() =>
      {
        try
        {
          StopCommandActivated = true;
          runningProcesses.Kill(background);
          if (VsServiceProvider.TryGetService(typeof(DTE), out object dte))
          {
            string solutionPath = (dte as DTE2).Solution.FullName;

            if (string.IsNullOrWhiteSpace(solutionPath))
              return;

            string solutionFolder = solutionPath.Substring(0, solutionPath.LastIndexOf('\\'));
            mPCHCleaner.Remove(solutionFolder);
          }
          mDirectoriesPath.Clear();
          StopCommandActivated = false;
        }
        catch (Exception e) 
        {
          MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
      });
    }

    #endregion


  }
}

