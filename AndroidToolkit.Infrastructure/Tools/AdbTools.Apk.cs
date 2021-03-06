﻿using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using AndroidToolkit.Infrastructure.Helpers;

namespace AndroidToolkit.Infrastructure.Tools
{
    public partial class AdbTools
    {
        public async Task InstallApk(string apk, bool isSystem, bool createNoWindow, string target = null)
        {
            apk = PathGenerator.Generate(apk);

            await Context.Dispatcher.InvokeAsync(async () =>
            {
                if (!isSystem)
                {
                    if (!string.IsNullOrEmpty(target))
                    {
                        await Task.Run(() => _executor.Execute(new Command(string.Format("adb -s {1} install {0} ", apk, target)), Context, createNoWindow));
                    }
                    else
                    {
                        await Task.Run(() => _executor.Execute(new Command(string.Format("adb install {0} ", apk)), Context, createNoWindow));
                    }
                }
                else
                {
                    await Task.Run(() => _executor.Execute(new Command(string.Format("adb -s {1} push {0} /system/app", apk, target)), Context, createNoWindow));
                }
            });
        }

        public async Task RemoveApk(string apk, bool isSystem, bool createNoWindow, string target = null)
        {
            apk = PathGenerator.Generate(apk);

            await Context.Dispatcher.InvokeAsync(async () =>
            {
                if (!isSystem)
                {
                    if (!string.IsNullOrEmpty(target))
                    {
                        await Task.Run(() => _executor.Execute(new Command(string.Format("adb -s {1} uninstall {0}", apk, target)), Context, createNoWindow));
                    }
                    else
                    {
                        await Task.Run(() => _executor.Execute(new Command(string.Format("adb uninstall {0}", apk)), Context, createNoWindow));
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(target))
                    {
                        await Task.Run(() => _executor.Execute(new Command(string.Format("adb -s {1} shell rm {0} /system/app", apk, target)), Context, createNoWindow));
                    }
                    else
                    {
                        await Task.Run(() => _executor.Execute(new Command(string.Format("adb shell rm {0} /system/app", apk)), Context, createNoWindow));
                    }
                }
            });
        }

        public async Task ListApps(TextBox context2, bool createNoWindow, string target = null)
        {
            await Context.Dispatcher.InvokeAsync(async () =>
            {
                if (!string.IsNullOrEmpty(target))
                {
                    await Task.Run(async () =>
                    {
                        string logcat =
                            StringLinesRemover.ForgetLastLine(
                                StringLinesRemover.RemoveLine(
                                    await
                                        _executor.Execute(
                                            new Command(string.Format("adb -s {0} shell pm list packages", target)),
                                            createNoWindow), 4));
                        await context2.Dispatcher.InvokeAsync(() => context2.Text = logcat);
                    });
                }
                else
                {
                    await Task.Run(async () =>
                    {
                        string logcat =
                            StringLinesRemover.ForgetLastLine(
                                StringLinesRemover.RemoveLine(
                                    await
                                        _executor.Execute(new Command(string.Format("adb shell pm list packages")),
                                            createNoWindow), 4));
                        await context2.Dispatcher.InvokeAsync(() => context2.Text = logcat);
                    });
                }
            });
        }
    }
}
