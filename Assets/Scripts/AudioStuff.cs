using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Circk {
	public class AudioStuff:MonoBehaviour {
		public static AudioStuff me = null;
		AudioSource audMusic;
		AudioSource audSfx;

		AudioClip newClip;
		float newClipCounter;

		static Dictionary<string,AudioClip> clipTable = null;

		void Awake() {
			me = this;
			audMusic = gameObject.AddComponent<AudioSource>();
			audMusic.loop = true;
			audSfx = gameObject.AddComponent<AudioSource>();
			audSfx.loop = false;

			newClip = null;
			newClipCounter = 0;

			if (clipTable == null) {
				clipTable = new Dictionary<string,AudioClip>();
				LoadClip("mus-menu");
				LoadClip("mus-fase");
				LoadClip("mus-vinheta");
			}
		}

		void Update() {
			if (newClipCounter > 0) {
				if (newClip == null) {
					newClipCounter = 0;
				} else if (audMusic.isPlaying) {
					newClipCounter -= Time.deltaTime*4;
					if (newClipCounter <= 0) {
						newClipCounter = 0;
						audMusic.volume = 1;
						audMusic.clip = newClip;
						newClip = null;
						audMusic.time = 0;
						audMusic.Play();
					} else {
						audMusic.volume = newClipCounter;
					}
				} else {
					newClipCounter = 0;
					audMusic.volume = 1;
					audMusic.clip = newClip;
					newClip = null;
					audMusic.time = 0;
					audMusic.Play();
				}
			}
		}

		public static void PlayMusic(string id) {
			me.newClip = GetClip(id);
			if (me.newClip == null) return;
			me.newClipCounter = 1;
		}

		public static void StopMusic() {
			me.audMusic.Stop();
		}

		public static void PlaySound(string id,float volume = .3f) {
			AudioClip aud = GetClip(id);
			if (aud == null) return;
			me.audSfx.PlayOneShot(aud,volume);
		}

		public static AudioClip LoadClip(string id) {
			var aud = Resources.Load<AudioClip>("Audio/"+id);
			if (aud == null) return null;
			clipTable.Add(id,aud);
			return aud;
		}

		public static AudioClip GetClip(string id) {
			AudioClip aud;
			if (clipTable.TryGetValue(id,out aud)) {
				return aud;
			}
			return LoadClip(id);
		}
	}
}