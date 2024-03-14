﻿using System.Threading;
using System.Collections.Generic;
using Verse;
using System.Linq;

namespace GameClient
{
    public static class DialogManager
    {
        //      inputCache
        // Any time a dialog that has inputs is left (it is popped from the stack or a new dialog is pushed)
        // ,it will save its own list of inputs to inputCache
        // inputs can also be manually set to save.
        public static List<object> inputCache;

        //      inputReserve
        //  Unlike inputCache, inputReserve never automatically gets updated, and must be set using
        //  the
        public static List<object> inputReserve;
        //an internal stack to keep track of windows
        //(this makes it easier and more accurate to find the last window pushed)
        private static Stack<Window> windowStack = new Stack<Window>();

        public static Window currentDialog;
        public static Window previousDialog;

        public static RT_WindowInputs currentDialogInputs;

        public static void PushNewDialog(Window window)
        {
            if (ClientValues.isReadyToPlay || Current.ProgramState == ProgramState.Entry)
            {
                try
                {
                    Logs.Message($"[Rimworld Together] > Pushing {window.ToString()}");

                    //Hide the current window
                    if (windowStack.Count > 0 )
                        Find.WindowStack.TryRemove(windowStack.Peek());

                    //add the new window to the internal stack
                    windowStack.Push(window);

                    //Get an instance of the new window as RT_WindowInputs so input info can be retrieved later
                    if (window is RT_WindowInputs) currentDialogInputs = (RT_WindowInputs)window;

                    //draw the new window
                    Find.WindowStack.Add(window);
                    ListWindows();
                }
                catch (System.Exception ex)
                {
                    Logs.Message(ex.ToString());
                }
            }
        }

        public static void PopInternalStack()
        {
            if (windowStack.Count > 0) windowStack.Pop();
        }

        public static void clearInternalStack()
        {
            Logs.Message("cleared window stack");
            windowStack.Clear();
        }

        public static void clearStack()
        {
            while (windowStack.Count > 0)
            {
                Logs.Message($"[Rimworld Together] > popping {windowStack.Peek().ToString()}");
                Find.WindowStack.TryRemove(windowStack.Pop(), true);
                if (windowStack.Count > 0)
                    Find.WindowStack.Add(windowStack.Peek());
            }
        }

        public static void PopDialog() {

            if (windowStack.Count > 0)
            {
                Logs.Message($"[Rimworld Together] > popping {windowStack.Peek().ToString()}");
                Find.WindowStack.TryRemove(windowStack.Pop(), true);
                if (windowStack.Count > 0) Find.WindowStack.Add(windowStack.Peek());
            }
        }

<<<<<<< HEAD
=======
        public static void PopDialog() 
        {

            if (windowStack.Count > 0)
            {
                Logs.Message($"[Rimworld Together] > popping {windowStack.Peek().ToString()}");

                Find.WindowStack.TryRemove(windowStack.Pop(), true);

                Find.WindowStack.Add(windowStack.Peek());
            }
            else
            {
                Logs.Message("[Rimworld Together] > Tried to pop a window from an empty stack");
            }
        }

>>>>>>> ec331b27ec35f907106b744ac4c8be0d17caf27f
        public static void PopDialog(Window window)
        {
            if (windowStack.Count > 0)
            {
                Logs.Message($"[Rimworld Together] > popping {windowStack.Peek().ToString()}");
                Find.WindowStack.TryRemove(windowStack.Pop(), true);
                if (windowStack.Count > 0) Find.WindowStack.Add(windowStack.Peek());
            }
        }

        public static void setInputReserve()
        {
            currentDialogInputs.CacheInputs();
            inputReserve = new List<object>(inputCache);
            Logs.Message($"[Rimworld Together] > Cached inputs for {currentDialogInputs}");
        }

        private static void ListWindows()
        {
            Window[] winArray = windowStack.ToArray();
            for (int i = winArray.Length-1; i >= 0;i--)
            {
                Logs.Message($"Window at {i} is {winArray[i]}");
            }
        }
    }
}
