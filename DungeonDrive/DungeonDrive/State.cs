﻿using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace DungeonDrive
{
    public abstract class State
    {
        public MainForm form;
        public State parent = null;
        public List<State> children = new List<State>();
        public bool covered = false;
        public bool paused = false;

        public State(MainForm form) { this.form = form; }

        public abstract void keyDown(object sender, KeyEventArgs e);
        public abstract void keyUp(object sender, KeyEventArgs e);
        public abstract void mouseDown(object sender, MouseEventArgs e);
        public abstract void mouseUp(object sender, MouseEventArgs e);
        public abstract void mouseMove(object sender, MouseEventArgs e);
        public abstract void paint(object sender, PaintEventArgs e);
        public abstract void tick(object sender, EventArgs e);

        public void addChildState(State state, bool cover, bool pause)
        {
            covered = cover;
            paused = pause;
            children.Add(state);
            state.parent = this;

            form.KeyDown -= this.keyDown;
            form.KeyUp -= this.keyUp;
            form.MouseDown -= this.mouseDown;
            form.MouseUp -= this.mouseUp;
            form.MouseMove -= this.mouseMove;

            if (cover)
                form.Paint -= this.paint;
            if (pause)
                form.timer.Tick -= this.tick;

            form.KeyDown += state.keyDown;
            form.KeyUp += state.keyUp;
            form.MouseDown += state.mouseDown;
            form.MouseUp += state.mouseUp;
            form.MouseMove += state.mouseMove;
            form.Paint += state.paint;
            form.timer.Tick += state.tick;

            form.Invalidate();
        }

        public void open()
        {
            form.KeyDown += this.keyDown;
            form.KeyUp += this.keyUp;
            form.MouseDown += this.mouseDown;
            form.MouseUp += this.mouseUp;
            form.MouseMove += this.mouseMove;
            form.Paint += this.paint;
            form.timer.Tick += this.tick;

            form.Invalidate();
        }

        public void close()
        {
            State[] childrenArray = children.ToArray();
            for (int i = 0; i < childrenArray.Length; i++)
                childrenArray[i].close();

            if (parent != null)
            {
                parent.children.Remove(this);
                if (parent.children.Count == 0)
                {
                    form.KeyDown += parent.keyDown;
                    form.KeyUp += parent.keyUp;
                    form.MouseDown += parent.mouseDown;
                    form.MouseUp += parent.mouseUp;
                    form.MouseMove += parent.mouseMove;

                    if (parent.covered)
                    {
                        form.Paint += parent.paint;
                        parent.covered = false;
                    }
                    if (parent.paused)
                    {
                        form.timer.Tick += parent.tick;
                        parent.paused = false;
                    }
                }
                parent = null;
            }

            form.KeyDown -= this.keyDown;
            form.KeyUp -= this.keyUp;
            form.MouseDown -= this.mouseDown;
            form.MouseUp -= this.mouseUp;
            form.MouseMove -= this.mouseMove;
            form.Paint -= this.paint;
            form.timer.Tick -= this.tick;

            form.Invalidate();
        }
    }
}