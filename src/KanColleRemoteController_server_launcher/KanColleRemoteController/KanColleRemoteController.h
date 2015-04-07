
// KanColleRemoteController.h : main header file for the PROJECT_NAME application
//

#pragma once

#ifndef __AFXWIN_H__
	#error "include 'stdafx.h' before including this file for PCH"
#endif

#include "resource.h"		// main symbols


// CKanColleRemoteControllerApp:
// See KanColleRemoteController.cpp for the implementation of this class
//

class CKanColleRemoteControllerApp : public CWinApp
{
public:
	CKanColleRemoteControllerApp();

// Overrides
public:
	virtual BOOL InitInstance();

// Implementation

	DECLARE_MESSAGE_MAP()
};

extern CKanColleRemoteControllerApp theApp;