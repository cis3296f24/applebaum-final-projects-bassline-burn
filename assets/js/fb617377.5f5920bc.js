"use strict";(self.webpackChunkcreate_project_docs=self.webpackChunkcreate_project_docs||[]).push([[1412],{12578:(e,a,n)=>{n.r(a),n.d(a,{assets:()=>m,contentTitle:()=>s,default:()=>v,frontMatter:()=>c,metadata:()=>t,toc:()=>o});const t=JSON.parse('{"id":"sequenceDiagrams","title":"Sequence Diagrams","description":"Sequence Diagram for Hosting a Game:","source":"@site/tutorial/sequenceDiagrams.md","sourceDirName":".","slug":"/sequenceDiagrams","permalink":"/applebaum-final-projects-bassline-burn/tutorial/sequenceDiagrams","draft":false,"unlisted":false,"tags":[],"version":"current","sidebarPosition":3,"frontMatter":{"sidebar_position":3},"sidebar":"docsSidebar","previous":{"title":"Understanding the Classes","permalink":"/applebaum-final-projects-bassline-burn/tutorial/understanding_classes"}}');var r=n(74848),i=n(28453);const c={sidebar_position:3},s="Sequence Diagrams",m={},o=[{value:"Sequence Diagram for Hosting a Game:",id:"sequence-diagram-for-hosting-a-game",level:3},{value:"Sequence Diagram for Joining a Game:",id:"sequence-diagram-for-joining-a-game",level:3},{value:"Sequence Diagram for Game Loop",id:"sequence-diagram-for-game-loop",level:3}];function g(e){const a={h1:"h1",h3:"h3",header:"header",mermaid:"mermaid",...(0,i.R)(),...e.components};return(0,r.jsxs)(r.Fragment,{children:[(0,r.jsx)(a.header,{children:(0,r.jsx)(a.h1,{id:"sequence-diagrams",children:"Sequence Diagrams"})}),"\n",(0,r.jsx)(a.h3,{id:"sequence-diagram-for-hosting-a-game",children:"Sequence Diagram for Hosting a Game:"}),"\n",(0,r.jsx)(a.mermaid,{value:"    sequenceDiagram\n    \n    User ->> Interface Manager: Load Game \n    Activate User\n    Activate Interface Manager\n    Activate Interface Manager\n    Interface Manager ->> Interface Manager: Fetch Client Info\n    Deactivate Interface Manager\n    \n    Interface Manager ->> CreateGameUI: Start()\n    Activate Interface Manager\n    Activate CreateGameUI\n    Activate CreateGameUI\n    CreateGameUI ->> CreateGameUI: SetTrack, LobbyName, Users\n    Deactivate CreateGameUI\n    CreateGameUI ->> Level Manager: Load Track\n    Activate Level Manager\n    Activate CreateGameUI\n\n    Level Manager ->> GameUI: Spawn Players\n    Activate GameUI\n    GameUI --) Level Manager: \n    Deactivate GameUI\n    Deactivate CreateGameUI\n    Level Manager --) CreateGameUI: \n    Deactivate Level Manager\n    Deactivate CreateGameUI\n    CreateGameUI --) Interface Manager: \n    Interface Manager --) User: \n    Deactivate Interface Manager\n    Deactivate Interface Manager\n    Deactivate User"}),"\n",(0,r.jsx)(a.h3,{id:"sequence-diagram-for-joining-a-game",children:"Sequence Diagram for Joining a Game:"}),"\n",(0,r.jsx)(a.mermaid,{value:"    sequenceDiagram\n    \n    User ->> Interface Manager: Load Game \n    Activate User\n    Activate Interface Manager\n    Activate Interface Manager\n    Interface Manager ->> Interface Manager: Fetch Client Info\n    Deactivate Interface Manager\n    \n    Interface Manager ->> JoinGameUI: Start()\n    Activate Interface Manager\n    Activate JoinGameUI\n    Activate JoinGameUI\n    JoinGameUI ->> JoinGameUI: Fetch Lobby Info\n    Deactivate JoinGameUI\n    JoinGameUI ->> Level Manager: Load Track\n    Activate Level Manager\n    Activate JoinGameUI\n\n    Level Manager ->> GameUI: Spawn Players\n    Activate GameUI\n    GameUI --) Level Manager: \n    Deactivate GameUI\n    Deactivate JoinGameUI\n    Level Manager --) JoinGameUI: \n    Deactivate Level Manager\n    Deactivate JoinGameUI\n    JoinGameUI --) Interface Manager: \n    Interface Manager --) User: \n    Deactivate Interface Manager\n    Deactivate Interface Manager\n    Deactivate User\n   \n\n    "}),"\n",(0,r.jsx)(a.h3,{id:"sequence-diagram-for-game-loop",children:"Sequence Diagram for Game Loop"}),"\n",(0,r.jsx)(a.mermaid,{value:"sequenceDiagram\n    \n   User ->> GameManager: ReadyState()\n   Activate User\n    Activate GameManager\n   GameManager ->> GameUI: StartCountdown()\n   Activate GameUI\n   Activate GameManager\n   GameUI --) GameManager: \n   Deactivate GameUI\n   Deactivate GameManager\n    Deactivate GameManager\n   GameManager --) User: \n   Deactivate User\n   User ->> GameUI: LapCount\n   Activate GameUI\n   Activate User\n   GameUI --) User: \n   Deactivate GameUI\n   Deactivate User\n   User ->> GameUI: Time Passed\n   Activate GameUI\n    Activate User\n   GameUI --) User: \n   Deactivate GameUI\n    Deactivate User\n\n   User ->> GameManager: Finished()\n   Activate GameManager\n   Activate User\n   GameManager ->> GameUI: Finished()\n   Activate GameUI\n   GameUI --) User: \n   Deactivate GameUI\n   Deactivate GameManager\n   Deactivate User"})]})}function v(e={}){const{wrapper:a}={...(0,i.R)(),...e.components};return a?(0,r.jsx)(a,{...e,children:(0,r.jsx)(g,{...e})}):g(e)}},28453:(e,a,n)=>{n.d(a,{R:()=>c,x:()=>s});var t=n(96540);const r={},i=t.createContext(r);function c(e){const a=t.useContext(i);return t.useMemo((function(){return"function"==typeof e?e(a):{...a,...e}}),[a,e])}function s(e){let a;return a=e.disableParentContext?"function"==typeof e.components?e.components(r):e.components||r:c(e.components),t.createElement(i.Provider,{value:a},e.children)}}}]);