"use strict";(self.webpackChunkcreate_project_docs=self.webpackChunkcreate_project_docs||[]).push([[2634],{46062:(e,a,t)=>{t.d(a,{A:()=>r});t(96540);var n=t(74848);function r(e){function a(){return a=e.id?e.id:(a=(a=(a=e.caption).replaceAll("."," ")).replaceAll(" ","-")).toLowerCase()}return(0,n.jsxs)("figure",{id:a(),align:e.align?e.align:"center",style:e.style?e.style:{},children:[e.children,e.src?(0,n.jsx)("img",{src:e.src,alt:e.alt}):(0,n.jsx)(n.Fragment,{}),(0,n.jsx)("figcaption",{align:e.align?e.align:"center",style:{fontWeight:"bold"},children:e.caption}),(0,n.jsx)("figcaption",{align:e.align?e.align:"center",style:{},children:e.subcaption})]})}},2045:(e,a,t)=>{t.r(a),t.d(a,{default:()=>A});t(96540);var n=t(34164),r=t(68957),i=t(22777),s=t(31054),c=t(74848),o=t(28453);function l(e){const a={a:"a",h1:"h1",h2:"h2",h3:"h3",header:"header",img:"img",mermaid:"mermaid",p:"p",...(0,o.R)(),...e.components};return(0,c.jsxs)(c.Fragment,{children:[(0,c.jsx)(a.p,{children:(0,c.jsx)(a.a,{href:"https://classroom.github.com/open-in-codespaces?assignment_repo_id=16892546",children:(0,c.jsx)(a.img,{src:"https://classroom.github.com/assets/launch-codespace-2972f46106e565e64193e422d61a12cf1da4916b45550586e14ef0a7c637dd04.svg",alt:"Open in Codespaces"})})}),"\n",(0,c.jsxs)("div",{align:"center",children:[(0,c.jsx)(a.header,{children:(0,c.jsx)(a.h1,{id:"bassline-burn",children:"Bassline Burn"})}),(0,c.jsxs)(a.p,{children:[(0,c.jsx)(a.a,{href:"https://temple-cis-projects-in-cs.atlassian.net/jira/software/c/projects/BASSB/issues?jql=project%20%3D%20%22BASSB%22%20ORDER%20BY%20created%20DESC",children:(0,c.jsx)(a.img,{src:"https://img.shields.io/badge/Report%20Issues-Jira-0052CC?style=flat&logo=jira-software",alt:"Report Issue on Jira"})}),"\n",(0,c.jsx)(a.a,{href:"https://github.com/cis3296f24/applebaum-final-projects-bassline-burn/actions/workflows/deploy.yml",children:(0,c.jsx)(a.img,{src:"https://github.com/ApplebaumIan/tu-cis-4398-docs-template/actions/workflows/deploy.yml/badge.svg",alt:"Deploy Docs"})}),"\n",(0,c.jsx)(a.a,{href:"https://cis3296f24.github.io/applebaum-final-projects-bassline-burn/",children:(0,c.jsx)(a.img,{src:"https://img.shields.io/badge/-Documentation%20Website-brightgreen",alt:"Documentation Website Link"})})]})]}),"\n",(0,c.jsx)(a.h2,{id:"keywords",children:"Keywords"}),"\n",(0,c.jsx)(a.p,{children:"Section 5, Top Down Pixel Driving Game made in Unity with C#."}),"\n",(0,c.jsx)(a.h2,{id:"project-abstract",children:"Project Abstract"}),"\n",(0,c.jsx)(a.p,{children:"This videogame introduces an interesting game mechanic of a live radio system affecting in game factors, like car max speed, acceleration, and turning power. To support this crisp mechanic, the game will feature a multiplayer system in which you and your friends can pick from an array of race courses based on real locations around the world and race eachother to see who is the best driver."}),"\n",(0,c.jsx)(a.h2,{id:"high-level-requirement",children:"High Level Requirement"}),"\n",(0,c.jsx)(a.p,{children:"This game has very simple parts but can get very complicated quickly. A lot of this is because of the multiplayer aspect of the game. For this reason, I think tackling the easy parts first like the base game then moving on to the multiplayer later will create a concise schedule that will allow us to make a smooth feeling game. On my own time I will attempt to create some art assests in order to make the game feel like our own and truly unique."}),"\n",(0,c.jsx)(a.h2,{id:"conceptual-design",children:"Conceptual Design"}),"\n",(0,c.jsx)(a.p,{children:"The conceptual design involves creating a basic multiplayer racing game with Unity and C# and then building off of that. Once when we create amazing controls and this state of the art radio system, we can build off of this by adding a lobby to hold our multiplayer in which racers can vote on maps and even add bots to their game if there are not enough players. While to start we will just have a simple host/join multiplayer system, I would like to add on a system that will add you to a random game that has some players in it already."}),"\n",(0,c.jsx)(a.h3,{id:"sequence-diagram-for-hosting-a-game",children:"Sequence Diagram for Hosting a Game:"}),"\n",(0,c.jsx)(a.mermaid,{value:"    sequenceDiagram\n    \n    User ->> Interface Manager: Load Game \n    Activate User\n    Activate Interface Manager\n    Activate Interface Manager\n    Interface Manager ->> Interface Manager: Fetch Client Info\n    Deactivate Interface Manager\n    \n    Interface Manager ->> CreateGameUI: Start()\n    Activate Interface Manager\n    Activate CreateGameUI\n    Activate CreateGameUI\n    CreateGameUI ->> CreateGameUI: SetTrack, LobbyName, Users\n    Deactivate CreateGameUI\n    CreateGameUI ->> Level Manager: Load Track\n    Activate Level Manager\n    Activate CreateGameUI\n\n    Level Manager ->> GameUI: Spawn Players\n    Activate GameUI\n    GameUI --) Level Manager: \n    Deactivate GameUI\n    Deactivate CreateGameUI\n    Level Manager --) CreateGameUI: \n    Deactivate Level Manager\n    Deactivate CreateGameUI\n    CreateGameUI --) Interface Manager: \n    Interface Manager --) User: \n    Deactivate Interface Manager\n    Deactivate Interface Manager\n    Deactivate User"}),"\n",(0,c.jsx)(a.h3,{id:"sequence-diagram-for-joining-a-game",children:"Sequence Diagram for Joining a Game:"}),"\n",(0,c.jsx)(a.mermaid,{value:"    sequenceDiagram\n    \n    User ->> Interface Manager: Load Game \n    Activate User\n    Activate Interface Manager\n    Activate Interface Manager\n    Interface Manager ->> Interface Manager: Fetch Client Info\n    Deactivate Interface Manager\n    \n    Interface Manager ->> JoinGameUI: Start()\n    Activate Interface Manager\n    Activate JoinGameUI\n    Activate JoinGameUI\n    JoinGameUI ->> JoinGameUI: Fetch Lobby Info\n    Deactivate JoinGameUI\n    JoinGameUI ->> Level Manager: Load Track\n    Activate Level Manager\n    Activate JoinGameUI\n\n    Level Manager ->> GameUI: Spawn Players\n    Activate GameUI\n    GameUI --) Level Manager: \n    Deactivate GameUI\n    Deactivate JoinGameUI\n    Level Manager --) JoinGameUI: \n    Deactivate Level Manager\n    Deactivate JoinGameUI\n    JoinGameUI --) Interface Manager: \n    Interface Manager --) User: \n    Deactivate Interface Manager\n    Deactivate Interface Manager\n    Deactivate User\n   \n\n    "}),"\n",(0,c.jsx)(a.h2,{id:"background",children:"Background"}),"\n",(0,c.jsx)(a.p,{children:"Growing up one of the most popular video games were Mariokart. Something about the fun controls made it entertaining for both the casual and hardcore racers. Now that I am a lot older, I want to replicate the game and combine it with my other interest in music. Live radios in games is not a new thing, from Forza to GTA, these games have all had in game radios. What will make this different is the fact that the radio actually affects the games and is not just an extra feature."}),"\n",(0,c.jsx)(a.h2,{id:"required-resources",children:"Required Resources"}),"\n",(0,c.jsx)(a.p,{children:"This project just requires Unity and an IDE to edit C# code in. There are a vast amount on optional resources, like a way to create art and music, and a platform to post the final game with."}),"\n",(0,c.jsx)(a.h2,{id:"collaborators",children:"Collaborators"}),"\n",(0,c.jsx)("table",{children:(0,c.jsxs)("tr",{children:[(0,c.jsx)("td",{align:"center",children:(0,c.jsxs)("a",{href:"https://github.com/ChristopherBrousseau",children:[(0,c.jsx)("img",{src:"https://avatars.githubusercontent.com/u/156946433?s=96&v=4",width:"100;",alt:"ChristopherBrousseau"}),(0,c.jsx)("br",{}),(0,c.jsx)("sub",{children:(0,c.jsx)("b",{children:"Christopher Brousseau"})})]})}),(0,c.jsx)("td",{align:"center",children:(0,c.jsxs)("a",{href:"https://github.com/glantig1",children:[(0,c.jsx)("img",{src:"https://avatars.githubusercontent.com/u/143743234?v=4&size=64",width:"100;",alt:"glantig1"}),(0,c.jsx)("br",{}),(0,c.jsx)("sub",{children:(0,c.jsx)("b",{children:"Gabriel Lantigua"})})]})}),(0,c.jsx)("td",{align:"center",children:(0,c.jsxs)("a",{href:"https://github.com/Random76520",children:[(0,c.jsx)("img",{src:"https://avatars.githubusercontent.com/u/123013478?s=400&v=4",width:"100;",alt:"Augustin"}),(0,c.jsx)("br",{}),(0,c.jsx)("sub",{children:(0,c.jsx)("b",{children:"Jonathan Augustin"})})]})}),(0,c.jsx)("td",{align:"center",children:(0,c.jsxs)("a",{href:"https://github.com/Gunlords",children:[(0,c.jsx)("img",{src:"https://avatars.githubusercontent.com/u/180465432?v=4",width:"100;",alt:"Ankur"}),(0,c.jsx)("br",{}),(0,c.jsx)("sub",{children:(0,c.jsx)("b",{children:"Ankur Chowdhury"})})]})}),(0,c.jsx)("td",{align:"center",children:(0,c.jsxs)("a",{href:"https://github.com/tus40499",children:[(0,c.jsx)("img",{src:"https://avatars.githubusercontent.com/u/157192065?v=4",width:"100;",alt:"JackMartin"}),(0,c.jsx)("br",{}),(0,c.jsx)("sub",{children:(0,c.jsx)("b",{children:"Jack Martin"})})]})})]})})]})}function h(e={}){const{wrapper:a}={...(0,o.R)(),...e.components};return a?(0,c.jsx)(a,{...e,children:(0,c.jsx)(l,{...e})}):l(e)}function d(){return(0,c.jsx)("div",{className:"container",style:{marginTop:"50px",marginBottom:"100px"},children:(0,c.jsx)(h,{})})}const m={heroBanner:"heroBanner_qdFl",buttons:"buttons_AeoN"};var u=t(4307);function g(){const{siteConfig:e}=(0,i.A)();return(0,c.jsx)("header",{className:(0,n.A)("hero hero--primary",m.heroBanner),children:(0,c.jsxs)("div",{className:"container",children:[(0,c.jsx)("h1",{className:"hero__title",children:e.title}),(0,c.jsx)("p",{className:"hero__subtitle",children:e.tagline}),(0,c.jsx)("div",{className:m.buttons,children:(0,c.jsx)(r.A,{className:"button button--secondary button--lg",to:"/tutorial/intro",children:"Docusaurus Tutoria"})})]})})}function A(){const{siteConfig:e}=(0,i.A)();return(0,c.jsxs)(s.A,{title:`Hello from ${e.title}`,description:"Description will go into a meta tag in <head />",children:[(0,c.jsx)(g,{}),(0,c.jsx)("main",{children:(0,c.jsx)(u.A,{children:(0,c.jsx)(d,{})})})]})}},4307:(e,a,t)=>{t.d(a,{A:()=>i});t(96540);var n=t(92180),r=t(74848);function i(e){return(0,r.jsx)(r.Fragment,{children:(0,r.jsx)(n.A,{...e})})}},77392:(e,a,t)=>{t.d(a,{A:()=>s});var n=t(96540),r=t(46062),i=t(74966);const s={React:n,...n,Figure:r.A,dinosaur:i.A}},74966:(e,a,t)=>{t.d(a,{A:()=>n});const n="data:image/png;base64,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"}}]);