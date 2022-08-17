﻿using ClangPowerTools;
using ClangPowerTools.Views;
using System.ComponentModel;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using ClangPowerToolsShared.MVVM.Models.ToolWindowModels;
using ClangPowerTools.MVVM.Command;
using System.Threading.Tasks;
using ClangPowerTools.Commands;
using ClangPowerToolsShared.MVVM.Controllers;
using System.Collections.ObjectModel;
using ClangPowerToolsShared.Commands;
using ClangPowerToolsShared.MVVM.Interfaces;
using ClangPowerToolsShared.MVVM.AutoCompleteHistory;

namespace ClangPowerToolsShared.MVVM.ViewModels
{

  public class FindToolWindowViewModel : FindController
  {
    private ASTMatchers astMatchers;
    public List<IViewMatcher> ViewMatchers
    {
      get { return FindToolWindowModel.ViewMatchers;  }
    }

    public List<string> ASTMatchers
    {
      get { return astMatchers.AutoCompleteMatchers; }
    }

    public FindToolWindowViewModel(FindToolWindowView findToolWindowView)
    {
      astMatchers = new();
      this.findToolWindowView = findToolWindowView;
    }

    public void OpenToolWindow() { }

    public void RunQuery()
    {
      if (!RunController.StopCommandActivated)
      {
        SelectCommandToRun(findToolWindowModel.CurrentViewMatcher);
        RunPowershellQuery();
      }
      AfterCommand();
    }

    public void SelectCommandToRun(IViewMatcher viewMatcher)
    {
      findToolWindowModel.UpdateUiToSelectedModel(viewMatcher);
      FindToolWindowModel = findToolWindowModel;
    }

    public void RunCommandFromView()
    {
      BeforeCommand();
      LaunchCommand();
      CommandControllerInstance.CommandController.LaunchCommandAsync(CommandIds.kClangFindRun, CommandUILocation.ContextMenu);

    }
  } 
}
