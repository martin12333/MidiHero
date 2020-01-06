﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MidiHero
{
	internal static class KeyboardForm
	{
		internal static SongForm Form;
		internal static int Track;
		internal static int Channel;
		internal static int Next;
		internal static Panel[] Panels;
		internal static System.Threading.Timer Timer;

		internal static void Show()
		{
			Form = new SongForm();

			Form.Text = Song.Name + " - Midi Hero v1.0";

			Form.FormClosing += Form_FormClosing;

			Form.Show();

			Panels = new Panel[88];
			var panels = new List<Panel>();

			for (var x = 0; x < Panels.Length; x++)
			{
				var panel = new Panel();

				var key = x % 12;
				var octave = x / 12;

				switch (key)
				{
					case 0:
						panel.Location = new Point(20 + (octave * 140) + 0, 280);
						panel.Size = new Size(19, 100);
						panel.BackColor = Color.White;

						panels.Add(panel);
						break;

					case 1:
						panel.Location = new Point(20 + (octave * 140) + 13, 280);
						panel.Size = new Size(13, 55);
						panel.BackColor = Color.DarkGray;

						Form.Controls.Add(panel);
						break;

					case 2:
						panel.Location = new Point(20 + (octave * 140) + 20, 280);
						panel.Size = new Size(19, 100);
						panel.BackColor = Color.White;

						panels.Add(panel);
						break;

					case 3:
						panel.Location = new Point(20 + (octave * 140) + 40, 280);
						panel.Size = new Size(19, 100);
						panel.BackColor = Color.White;

						panels.Add(panel);
						break;

					case 4:
						panel.Location = new Point(20 + (octave * 140) + 53, 280);
						panel.Size = new Size(13, 55);
						panel.BackColor = Color.DarkGray;

						Form.Controls.Add(panel);
						break;

					case 5:
						panel.Location = new Point(20 + (octave * 140) + 60, 280);
						panel.Size = new Size(19, 100);
						panel.BackColor = Color.White;

						panels.Add(panel);
						break;

					case 6:
						panel.Location = new Point(20 + (octave * 140) + 73, 280);
						panel.Size = new Size(13, 55);
						panel.BackColor = Color.DarkGray;

						Form.Controls.Add(panel);
						break;

					case 7:
						panel.Location = new Point(20 + (octave * 140) + 80, 280);
						panel.Size = new Size(19, 100);
						panel.BackColor = Color.White;

						panels.Add(panel);
						break;

					case 8:
						panel.Location = new Point(20 + (octave * 140) + 100, 280);
						panel.Size = new Size(19, 100);
						panel.BackColor = Color.White;

						panels.Add(panel);
						break;

					case 9:
						panel.Location = new Point(20 + (octave * 140) + 113, 280);
						panel.Size = new Size(13, 55);
						panel.BackColor = Color.DarkGray;

						Form.Controls.Add(panel);
						break;

					case 10:
						panel.Location = new Point(20 + (octave * 140) + 120, 280);
						panel.Size = new Size(19, 100);
						panel.BackColor = Color.White;

						panels.Add(panel);
						break;

					case 11:
						panel.Location = new Point(20 + (octave * 140) + 133, 280);
						panel.Size = new Size(13, 55);
						panel.BackColor = Color.DarkGray;

						Form.Controls.Add(panel);
						break;
				}

				Panels[x] = panel;
			}

			Form.Controls.AddRange(panels.ToArray());

			Next = 0;

			Timer = new System.Threading.Timer(Timer_Callback, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(10));
		}

		private static void Form_FormClosing(object sender, FormClosingEventArgs e)
		{
			SongPlayer.Stop();

			Timer.Change(-1, -1);
			Timer.Dispose();

			Form = null;
		}

		private static void Timer_Callback(object state)
		{
			try
			{
				if (Form != null &&
					!Form.Disposing &&
					!Form.IsDisposed)
					Form.Invoke((Action)Update);
			}
			catch (Exception)
			{
			}
		}

		internal static void Hide()
		{
			Form.Hide();

			Form.Dispose();
			Form = null;
		}

		internal static void Update()
		{
			while (Next < SongPlayer.Next[Track])
			{
				var e = Song.Tracks[Track].Events[Next];

				if(Channel == e.Channel)
				{
					if (e.Type == Song.EventType.NoteOn)
					{
						if (e.Value >= 21 &&
							e.Value < 109)
						{
							var key = (e.Value - 21) % 12;

							switch (key)
							{
								case 0:
								case 2:
								case 3:
								case 5:
								case 7:
								case 8:
								case 10:
									Panels[e.Value - 21].BackColor = e.Value2 != 0 ? Color.Lime : Color.White;
									break;

								default:
									Panels[e.Value - 21].BackColor = e.Value2 != 0 ? Color.Lime : Color.DarkGray;
									break;
							}
						}
					}
					else if (e.Type == Song.EventType.NoteOff)
					{
						if (e.Value >= 21 &&
							e.Value < 109)
						{
							var key = (e.Value - 21) % 12;

							switch (key)
							{
								case 0:
								case 2:
								case 3:
								case 5:
								case 7:
								case 8:
								case 10:
									Panels[e.Value - 21].BackColor = Color.White;
									break;

								default:
									Panels[e.Value - 21].BackColor = Color.DarkGray;
									break;
							}
						}
					}
				}

				Next++;
			}
		}

		//internal static void Update()
		//{
		//	for (int x = 0; x < Guitar.Tuning.Length; x++)
		//		for (int y = 0; y < 24; y++)
		//			Panels[x, y].BackColor = Color.Gray;

		//	if (SongPlayer.Playing[Track])
		//	{
		//		var next = SongPlayer.Next[Track];
		//		var e = Song.Tracks[Track].Events[Math.Max(next - 1, 0)];

		//		for (int x = 0; x < Guitar.Tuning.Length; x++)
		//		{
		//			if (e.Type == Song.EventType.NoteOn &&
		//				e.Value2 != 0)
		//			{
		//				if (e.Value >= Guitar.Tuning[x] &&
		//					e.Value < Guitar.Tuning[x] + 24)
		//				{
		//					Panels[x, e.Value - Guitar.Tuning[x]].BackColor = Color.Lime;
		//					//break;
		//				}
		//			}
		//			//else if (e.Type == Song.EventType.NoteOff)
		//			//{
		//			//	if (e.Value >= Guitar.Tuning[x] &&
		//			//		e.Value < Guitar.Tuning[x] + 24)
		//			//	{
		//			//		Panels[x, e.Value - Guitar.Tuning[x]].BackColor = Color.Gray;
		//			//		break;
		//			//	}
		//			//}
		//		}
		//	}
		//}
	}
}