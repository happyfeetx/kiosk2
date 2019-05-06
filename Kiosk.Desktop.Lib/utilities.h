#pragma once

#define WIN32_LEAN_AND_MEAN             // Exclude rarely-used stuff from Windows headers
// Windows Header Files
#include <windows.h>
#include <string>

namespace Kiosk {
	namespace Utilities {
		class Disposable {
		public:


			Disposable() {

			}

			virtual ~Disposable();

		private:
			~Disposable() {

			}
		};
	}
}
