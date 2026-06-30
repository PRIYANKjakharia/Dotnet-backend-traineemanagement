import java.util.*;

public class Main {
    public static void main(String[] args) {
      Scanner sc = new Scanner(System.in);
      
      while(true){
        System.out.println("Enter Your Choice: ");
        System.out.println(" (1) Open TaskManager ");
        System.out.println(" (2) Exit ");
        System.out.println();
        int input = 0;
        try{
          input = sc.nextInt();
        }catch(Exception e){
          System.out.println("Invalid input");
        }
        
        if(input == 2){
          break;
        }else if(input == 1){
          TaskManager tm = new TaskManager();
          break;
        }else{
          System.out.println("Invalid input , try again");
          System.out.println();
        }
        
      }
      
      
      // TaskManager tm = new TaskManager();
      // tm.addTask("trial 1" , true);
      // tm.addTask("trial 2" , false);
      // tm.addTask("trial 3" , true);
      // tm.addTask("trial 4");
      // tm.addTask("trial 5");
      
      // tm.deleteTask("id-5");
      // tm.displayAllTasks();
    }
}
class Task{
  
  private boolean status;
  private String description;
  Scanner sc = new Scanner(System.in);
  
  Task(){
    status = false;
  }
  
  Task(String description){
    this.description = description;
    status = false;
  }
  
  Task(String description , boolean status){
    this.description = description;
    this.status = status;
  }
  
  // Task getter setters
  public String getStatus(){
    return status ? "COMPLETED" : "PENDING";
  }
  public void setStatus(boolean status){
    this.status = status;
  }
  // Desctiprion getter setters
  public String getDescription(){
    return description;
  }
  public void setDescription(String description){
    this.description = description;
  }
  
  // returning string format of an individual task
  public String toString(){
    return "Task: "+description+"   Status: "+ getStatus();
  }
}

class TaskManager{
  private HashMap< String , Task > taskMap;
  // private TreeMap< String , Task > taskMap;
  private static int id;
  Scanner sc = new Scanner(System.in);
  Random r= new Random();
  
  TaskManager(){
    System.out.println("Opened TaskManager");
    taskMap = new HashMap<>();
    // taskMap = new TreeMap<>();
    id = 0;
    
    
    while(true){
      System.out.println("Choose desired operation");
      System.out.println(" (1) add Task");
      System.out.println(" (2) delete Task");
      System.out.println(" (3) update Task status");
      System.out.println(" (4) display all tasks");
      System.out.println(" (5) exit");
      System.out.println();
      int input = 4;
      try{
        input = sc.nextInt();
      }catch(Exception e){
        System.out.println("invalid input");
        break;
      }
      
      if(input == 5){
        break;
      }else if(input == 1){
        System.out.println("enter task description");
        String taskDescription = sc.next();
        System.out.println("enter task status ( c - completed , p - pending , s - skip )");
        String taskStatus = sc.next();
        if(taskStatus.equals("s")){
          addTask(taskDescription);
        }else{
          boolean statusFlag = taskStatus.equals("c") ? true : false;
          addTask(taskDescription , statusFlag);
        }
      }else if(input == 2){
          System.out.println("enter task id");
          String tid = sc.next();
          if(!taskMap.containsKey(tid)){
            System.out.println("Task with taskid: "+ tid+" does not exist");
          }else {
            deleteTask(tid);
          }
      }else if(input == 3){
          System.out.println("enter task id");
          String tid = sc.next();
          if(!taskMap.containsKey(tid)){
            System.out.println("Task with taskid: "+ tid+" does not exist");
          }else {
            System.out.println("enter task status ( c - completed , p - pending )");
            String taskStatus = sc.next();
            boolean statusFlag = taskStatus.equals("c") ? true : false;
            updateTask(tid , statusFlag);
          }
      }else if(input == 4){
          displayAllTasks();
      }else{
        System.out.println("Invalid input , try again");
      }
    }
  }
  
  // adding tasks
  void addTask( String description , boolean status ){
    Task task = new Task( description , status );
    addToTaskMap(task);
  }
  void addTask( String description ){
    Task task = new Task( description );
    addToTaskMap(task);
  }
  void addToTaskMap( Task task ){
    int offset = r.nextInt(10);
    taskMap.put( "id-"+ (id+offset) , task );
    System.out.println("Task created with taskId id-"+ (id+offset) );
    id+= 10;
  }
  
  
  // deleting tasks
  void deleteTask( String taskId ){
    taskMap.remove(taskId);
    System.out.println("Task with taskId "+taskId+" has been deleted");
  }
  
  //update tasks
  void updateTask( String taskId , boolean status){
    Task task = taskMap.get(taskId);
    // task.setDescription(description);
    task.setStatus(status);
  }
  
  // displaying all tasks
  void displayAllTasks(){
    System.out.println();
    System.out.println("DISPLAYING ALL TASKS");
    System.out.println();
    for(String taskId : taskMap.keySet()){
      System.out.println("TASK ID: "+taskId+"    "+taskMap.get(taskId));
    }
    System.out.println();
    System.out.println();
  }
  
}